using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.BaseService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using static ProjectManagement.Domain.Enums.EnumExtensions;

namespace ProjectManagement.Service.Service
{
    public class OtpService : BaseService<Otp>, IOtpService
    {
        #region Fields
        private readonly IOtpRepository _otpRepository;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        #endregion

        #region Constructor
        public OtpService(IStringLocalizer<AppLocalization> stringLocalizer,
                         IOtpRepository otpRepository,
                         IUserService userService,
                         IEmailService emailService) : base(otpRepository, stringLocalizer)
        {
            _otpRepository = otpRepository;
            _userService = userService;
            _emailService = emailService;
        }
        #endregion

        #region Methods
        public async Task<string?> GenerateOtpAsync(User user)
        {
            var otp = new Otp
            {
                Email = user.Email,
                OtpCode = new Random().Next(10000, 99999).ToString(),
                ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };
            await AddAsync(otp);
            return otp.OtpCode;
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

            return _stringLocalizer[AppLocalizationKeys.Success];
        }
        public async Task<bool> VerifyOtpAsync(User user, string otp)
        {
            if (string.IsNullOrEmpty(user.Email!) || string.IsNullOrEmpty(otp))
                return false;
            var Otp = await _otpRepository.GetTableNoTracking()
                        .Where(x => x.Email == user.Email && x.OtpCode == otp && x.IsUsed == false && x.ExpiryTime > DateTime.UtcNow)
                        .FirstOrDefaultAsync();

            if (Otp == null)
                return false;

            if (Otp.IsUsed == true)
                return false;

            await MarkAsUsedAsync(otp, user.Email!);
            return true;
        }
        public async Task<string> MarkAsUsedAsync(string Otp, string Email)
        {
            var OtpReocrd = await _otpRepository
                                .GetTableAsTracking()
                                .Where(x => x.OtpCode == Otp && x.Email == Email && x.IsUsed == false && x.ExpiryTime > DateTime.UtcNow)
                                .FirstOrDefaultAsync();
            OtpReocrd!.IsUsed = true;
            var response = EditAsync(OtpReocrd);
            if (response.Result != _stringLocalizer[AppLocalizationKeys.Success])
                return _stringLocalizer[AppLocalizationKeys.FailedToVerifyOtp];

            return _stringLocalizer[AppLocalizationKeys.Success];
        }

        public async Task<bool> RemoveLastOtp(string Email)
        {
            try
            {
                var Rows = _otpRepository.GetTableAsTracking()
                                         .Where(x => x.Email == Email).ToList();

                await _otpRepository.DeleteRangeAsync(Rows);
                return true;
            }
            catch
            {
                return false;
            }
        }


        #endregion
    }
}
