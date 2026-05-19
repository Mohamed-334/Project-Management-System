using ProjectManagement.Domain.Shared.BaseEntity.Implementations;

namespace ProjectManagement.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string? Title { get; set; }
        public string? Message { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SendAt { get; set; }

        public virtual ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
    }
}
