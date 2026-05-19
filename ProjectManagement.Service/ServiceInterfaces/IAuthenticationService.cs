using ProjectManagement.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static ProjectManagement.Domain.Enums.EnumExtensions;

namespace ProjectManagement.Service.ServiceInterfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> SignUpAsync(User user, string password);
        Task<IdentityResult> ExternalSignUpAsync(User user);
        Task SignInAsync(User user, bool IsPersistent);
        Task<(JwtSecurityToken, string)> GenerateTokenAsync(User user);
        Task<string> SetTokenInCookieAsync(User user);
        Task<List<Claim>> GetClaimsAsync(User user);
        Task<SignInResult> CheckSignInPasswordAsync(User user, string? Password, bool LockedOnFailure);
        Task<string> Logout();
        Task<IdentityResult> ChangePasswordAsync(User user, string CurrentPassword, string NewPassword);
        Task<string> GenerateOtpAsync(User user);
        Task<string> SendOtpAsync(User user, OtpReasonEnum reason);
        Task<bool> VerifyOtpAsync(User user, string otp);
        Task<IdentityResult> ResetPasswordAsync(User user, string password);
        Task<IdentityResult> AddExternalLoginAsync(User user, UserLoginInfo loginInfo);
        Task<User?> FindByLoginAsync(UserLoginInfo loginInfo);
        Task<ExternalLoginInfo?> GetExternalAuthenticationInfoAsync();
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirect);
        Task<SignInResult> ExternalAuthenticationSignInAsync(string provider, string providerKey);
        Task<IList<UserLoginInfo>> GetLoginsAsync(User user);

    }
}
