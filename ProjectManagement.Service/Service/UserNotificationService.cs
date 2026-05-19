using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.BaseService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Service.Service
{
    public class UserNotificationService : BaseService<UserNotification>, IUserNotificationService
    {
        #region Fields
        private readonly IUserNotificationRepository _userNotificationRepository;
        #endregion

        #region Constructor
        public UserNotificationService(IStringLocalizer<AppLocalization> stringLocalizer, IUserNotificationRepository userNotificationRepository) : base(userNotificationRepository, stringLocalizer)
        {
            _userNotificationRepository = userNotificationRepository;
        }
        #endregion

        #region Methods
        public async Task<string> MarkNotificationsAsReadAsync(int userId)
        {
            var userNotifications = await _userNotificationRepository
                                        .GetTableNoTracking()
                                        .Where(un => un.UserId == userId && !un.IsRead)
                                        .ToListAsync();

            foreach (var userNotification in userNotifications)
            {
                userNotification.IsRead = true;
            }
            var trans = _userNotificationRepository.BeginTransaction();
            try
            {
                await _userNotificationRepository.UpdateRangeAsync(userNotifications);
                await trans.CommitAsync();
                return _stringLocalizer[AppLocalizationKeys.Success];
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return _stringLocalizer[AppLocalizationKeys.UpdateFailed];
            }
        }
        public async Task<string> MarkNotificationAsReadAsync(int userId, int notificationId)
        {
            var userNotification = await _userNotificationRepository
                .GetTableNoTracking()
                .FirstOrDefaultAsync(un => un.NotificationId == notificationId && un.UserId == userId);

            if (userNotification == null)
                return _stringLocalizer[AppLocalizationKeys.NotFound];

            userNotification.IsRead = true;

            var result = await EditAsync(userNotification);
            return result;
        }

        public async Task<List<Notification?>> GetUserNotificationsAsync(int userId)
        {
            var userNotifications = await _userNotificationRepository
                .GetTableNoTracking()
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId)
                .OrderByDescending(un => un.Notification!.CreatedAt)
                .Select(n => n.Notification)
                .ToListAsync();

            return userNotifications;
        }
        #endregion
    }
}
