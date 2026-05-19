using ProjectManagement.Domain.Shared.BaseEntity.Implementations;

namespace ProjectManagement.Domain.Entities
{
    public class UserNotification : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public int NotificationId { get; set; }
        public virtual Notification? Notification { get; set; }
        public bool IsRead { get; set; }
    }
}
