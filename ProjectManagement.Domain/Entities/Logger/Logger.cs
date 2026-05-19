namespace ProjectManagement.Domain.Entities.Logger
{
    public class Logger
    {
        public int Id { get; set; }
        public string? TableName { get; set; } = "";
        public string? Action { get; set; } = "";
        public string? KeyValues { get; set; } = "";
        public string? OldValues { get; set; } = "";
        public string? NewValues { get; set; } = "";
        public int? UserId { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
