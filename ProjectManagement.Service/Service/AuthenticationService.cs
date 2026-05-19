using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Shared.JwtModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static ProjectManagement.Domain.Enums.EnumExtensions;
using IAuthenticationService = ProjectManagement.Service.ServiceInterfaces.IAuthenticationService;

namespace ProjectManagement.Service.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signManager;
        private readonly JwtSettings _jwtSettings;
        #endregion

        #region Constructor
        public AuthenticationService(UserManager<User> userManager, IStringLocalizer<AppLocalization> stringLocalizer, SignInManager<User> signManager, JwtSettings jwtSettings, IEmailService emailService, IOtpService otpService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
            _signManager = signManager;
            _jwtSettings = jwtSettings;
            _emailService = emailService;
            _otpService = otpService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods

        public async Task<IdentityResult> SignUpAsync(User user, string password) => await _userManager.CreateAsync(user, password);
        public async Task<IdentityResult> ExternalSignUpAsync(User user) => await _userManager.CreateAsync(user);
        public async Task SignInAsync(User user, bool IsPersistent) => await _signManager.SignInAsync(user, IsPersistent);
        public async Task<SignInResult> CheckSignInPasswordAsync(User user, string? Password, bool LockedOnFailure) => await _signManager.CheckPasswordSignInAsync(user, Password, LockedOnFailure);
        private string EncryptedToken(string token)
        {
            var Prefix = RandomStringGenerator.Generate(15);
            var Suffix = RandomStringGenerator.Generate(10);
            var CombinedToken = $"{Prefix}{token}{Suffix}";
            var EncryptedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(CombinedToken));
            return EncryptedToken;
        }
        public async Task<(JwtSecurityToken, string)> GenerateTokenAsync(User user)
        {
            var claims = await GetClaimsAsync(user);
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var encryptedToken = EncryptedToken(accessToken);
            return (jwtToken, encryptedToken);
        }
        public async Task<string> SetTokenInCookieAsync(User user)
        {
            var Token = (await GenerateTokenAsync(user)).Item2;

            var context = _httpContextAccessor.HttpContext;
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpireDate),
            };
            context?.Response.Cookies.Append("AccessToken", Token, cookieOptions);
            return Token;
        }
        public async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.GivenName, user.Name ?? ""),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            return claims;
        }
        public async Task<string> Logout()
        {
            var context = _httpContextAccessor.HttpContext;
            context?.Response.Cookies.Delete("AccessToken");
            await _signManager.SignOutAsync();
            return _stringLocalizer[AppLocalizationKeys.Success];
        }
        public async Task<IdentityResult> ChangePasswordAsync(User user, string CurrentPassword, string NewPassword)
                                                             => await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
        public async Task<string> GenerateOtpAsync(User user) => await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
        public async Task<bool> VerifyOtpAsync(User user, string otp)
        {
            var result = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, otp);
            await _userManager.UpdateSecurityStampAsync(user);
            var EditOtp = await _otpService.MarkAsUsedAsync(otp, user.Email!);
            return result;
        }


        public async Task<string> SendOtpAsync(User user, OtpReasonEnum reason)
        {
            var OtpCode = await GenerateOtpAsync(user);
            var msg = "";
            var Title = "";
            if (OtpCode == null)
                return _stringLocalizer[AppLocalizationKeys.FailedToGenerateOtp];
            switch (reason)
            {
                case OtpReasonEnum.Register:
                    msg = $"Hello {user.UserName},\n\n" +
                        $"Your OTP confirmation code is: {OtpCode}\n\n" +
                        "This code will expire in 5 minutes. If you did not request it, please ignore this email.\n\n" +
                        "Best regards,\nYour App Team";
                    Title = "Otp Confirmation";
                    break;
                case OtpReasonEnum.ResetPassword:
                    msg = $"Hello {user.UserName},\n\n" +
                                $"We received a request to reset your password.\n" +
                                $"Your One-Time Password (OTP) is: {OtpCode}\n\n" +
                                $"This OTP will expire in 5 minutes.\n" +
                                $"If you did not request this, please ignore this message.\n\n" +
                                $"Best regards,\n" +
                                $" Team";
                    Title = "Password Reset OTP";
                    break;
                case OtpReasonEnum.Resend:
                    msg = $"Hello {user.UserName},\n\n" +
                           $"Your OTP confirmation code is: {OtpCode}\n\n" +
                           "This code will expire in 5 minutes. If you did not request it, please ignore this email.\n\n" +
                           "Best regards,\nYour App Team";
                    Title = "New Otp Confirmation";
                    break;
            }
            var emailResult = await _emailService.SendEmailAsync(user.Email!, msg, Title);
            if (emailResult != _stringLocalizer[AppLocalizationKeys.Success])
                return _stringLocalizer[AppLocalizationKeys.FailedToGenerateOtp];
            var AddOtpResult = await _otpService.AddAsync(new Otp
            {
                CreationDate = DateTime.Now,
                ExpiryTime = DateTime.Now.AddMinutes(5),
                Email = user.Email,
                IsUsed = false,
                OtpCode = OtpCode,
            });
            return _stringLocalizer[AppLocalizationKeys.Success];
        }
        public async Task<IdentityResult> ResetPasswordAsync(User user, string password)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return result;
        }
        public async Task<IdentityResult> AddExternalLoginAsync(User user, UserLoginInfo loginInfo) => await _userManager.AddLoginAsync(user, loginInfo);
        public async Task<User?> FindByLoginAsync(UserLoginInfo loginInfo) => await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);

        public async Task<ExternalLoginInfo?> GetExternalAuthenticationInfoAsync() => await _signManager.GetExternalLoginInfoAsync();

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirect)
                                => _signManager.ConfigureExternalAuthenticationProperties(provider, redirect);

        public async Task<SignInResult> ExternalAuthenticationSignInAsync(string provider, string providerKey) =>
                    await _signManager.ExternalLoginSignInAsync(provider, providerKey, isPersistent: false);

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            return await _userManager.GetLoginsAsync(user);
        }
        #endregion
    }
}
