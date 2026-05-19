using ProjectManagement.Domain.Entities.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ProjectManagement.Infrastructure.Context.Interceptors
{
    public class LoggerSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly List<PendingLog> _pendingLogs = new();

        public LoggerSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private record PendingLog(EntityEntry Entry, string Action, Dictionary<string, object>? OldValues, Dictionary<string, object>? NewValues);

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .Where(e => e.Entity.GetType().Name != nameof(Logger))
                .ToList();

            foreach (var entry in entries)
            {
                Dictionary<string, object>? oldValues = null;
                Dictionary<string, object>? newValues = null;

                if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    oldValues = entry.Properties
                        .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue)!;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    newValues = entry.Properties
                        .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue)!;
                }

                _pendingLogs.Add(new PendingLog(entry, entry.State.ToString(), oldValues, newValues));
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null || !_pendingLogs.Any())
                return base.SavedChangesAsync(eventData, result, cancellationToken);

            var auditEntries = new List<Logger>();

            foreach (var log in _pendingLogs)
            {
                if (log.Entry.Properties.Any(p => p.Metadata.IsPrimaryKey()))
                {
                    var pkName = log.Entry.Properties.First(p => p.Metadata.IsPrimaryKey()).Metadata.Name;
                    var realId = log.Entry.Property(pkName).CurrentValue;

                    if (log.OldValues != null && log.OldValues.ContainsKey(pkName))
                        log.OldValues[pkName] = realId;

                    if (log.NewValues != null && log.NewValues.ContainsKey(pkName))
                        log.NewValues[pkName] = realId;
                }

                var keyValues = JsonConvert.SerializeObject(log.Entry.Properties
                    .Where(p => p.Metadata.IsPrimaryKey())
                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));

                var audit = new Logger
                {
                    TableName = log.Entry.Entity.GetType().Name,
                    Action = log.Action,
                    KeyValues = keyValues,
                    OldValues = log.OldValues != null ? JsonConvert.SerializeObject(log.OldValues) : null,
                    NewValues = log.NewValues != null ? JsonConvert.SerializeObject(log.NewValues) : null,
                    UserId = GetUserId(),
                    DateTime = DateTime.UtcNow
                };

                auditEntries.Add(audit);
            }

            if (auditEntries.Any())
            {
                context.Set<Logger>().AddRange(auditEntries);
                context.SaveChanges();
            }

            _pendingLogs.Clear();

            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        private int? GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity!.IsAuthenticated)
                return null;

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
        }
    }
}
