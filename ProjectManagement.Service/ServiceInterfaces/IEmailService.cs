namespace ProjectManagement.Service.ServiceInterfaces
{
    public interface IEmailService
    {
        Task<string> SendEmailAsync(string email, string Message, string? reason);
    }
}
