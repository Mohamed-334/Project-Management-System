using ProjectManagement.Domain.Entities;
using ProjectManagement.Service.Shared.Interface;

namespace ProjectManagement.Service.ServiceInterfaces
{
    public interface IUserNotificationService : IBaseService<UserNotification>
    {
        Task<string> MarkNotificationsAsReadAsync(int userId);
        Task<string> MarkNotificationAsReadAsync(int userId, int notificationId);
        Task<List<Notification?>> GetUserNotificationsAsync(int userId);
    }
}
