using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Hubs;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.BaseService;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Service.Service
{
    public class NotificationService : BaseService<Notification>, INotificationService
    {
        #region Fields
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        #endregion

        #region Constructor
        public NotificationService(IStringLocalizer<AppLocalization> stringLocalizer,
                                   INotificationRepository notificationRepository,
                                   IHubContext<NotificationHub> hubContext) : base(notificationRepository, stringLocalizer)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }
        #endregion

        #region Methods
        public Task SendNotificationToUsers(Notification notification, List<int> Users)
        {
            return _hubContext.Clients.Users(Users.Select(u => u.ToString()).ToList())
                                .SendAsync("ReceiveNotification", notification, CancellationToken.None);
        }
        public async Task<string> SendNotification(Notification? notification, List<int> Users)
        {
            if (notification == null)
                return _stringLocalizer[AppLocalizationKeys.NotFound];
            var AddResult = await AddAsync(notification);
            if (AddResult != _stringLocalizer[AppLocalizationKeys.Success])
                return _stringLocalizer[AppLocalizationKeys.AddFailed];

            try
            {
                BackgroundJob.Schedule<NotificationService>(AddResult => AddResult.SendNotificationToUsers(notification, Users), notification.SendAt!.Value);

                return _stringLocalizer[AppLocalizationKeys.Success];
            }
            catch (Exception ex)
            {
                return _stringLocalizer[AppLocalizationKeys.FailedToSendNotification];
            }

        }
        #endregion
    }
}
