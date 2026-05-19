using ProjectManagement.Domain.Entities;
using ProjectManagement.Service.Shared.Interface;

namespace ProjectManagement.Service.ServiceInterfaces
{
    public interface INotificationService : IBaseService<Notification>
    {
        Task<string> SendNotification(Notification? notification, List<int> Users);
    }
}
