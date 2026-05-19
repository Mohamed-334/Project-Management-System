namespace ProjectManagement.Core.Features.Notifications.Dto
{
    public class NotificationFullDataDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? TitleLocalized { get; set; }
        public string? Message { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
