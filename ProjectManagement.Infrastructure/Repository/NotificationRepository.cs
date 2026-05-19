using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Context;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.BaseRepository;
using ProjectManagement.Infrastructure.Shared.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Infrastructure.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        #region Fields
        private readonly DbSet<Notification> _otp;
        private readonly IStringLocalizer<AppLocalization> _localizer;

        #endregion

        #region Constructor
        public NotificationRepository(AppDbContext context, IStringLocalizer<AppLocalization> localizer) : base(context)
        {
            _otp = context.Set<Notification>();
            _localizer = localizer;
        }
        #endregion

        #region Actions

        #endregion
    }
}
