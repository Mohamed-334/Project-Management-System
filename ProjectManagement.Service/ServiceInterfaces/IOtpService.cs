using ProjectManagement.Domain.Entities;
using ProjectManagement.Service.Shared.Interface;
using static ProjectManagement.Domain.Enums.EnumExtensions;

namespace ProjectManagement.Service.ServiceInterfaces
{
    public interface IOtpService : IBaseService<Otp>
    {
        Task<string?> GenerateOtpAsync(User user);
        Task<string> SendOtpAsync(User user, OtpReasonEnum reason);
        Task<bool> VerifyOtpAsync(User user, string otp);
        Task<string> MarkAsUsedAsync(string Otp, string Email);
    }
}
