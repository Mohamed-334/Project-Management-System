namespace ProjectManagement.Domain.Shared.EmailModels
{
    public class EmailSettings
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Password { get; set; }
    }
}
