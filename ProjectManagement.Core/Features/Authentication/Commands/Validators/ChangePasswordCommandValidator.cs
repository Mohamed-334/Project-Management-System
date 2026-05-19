using ProjectManagement.Core.Features.Authentication.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Authentication.Commands.Validators
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IUserService _userService;
        #endregion

        #region Constructor
        public ChangePasswordCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer, IUserService userService)
        {
            _stringLocalizer = stringLocalizer;
            _userService = userService;
            ApplySignUpCommandValidation();
            ApplyCustomSignUpCommandValidation();
        }
        #endregion

        #region Methods

        public void ApplySignUpCommandValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword).WithMessage(_stringLocalizer[AppLocalizationKeys.PasswordNotEqualConfirmPass]);
        }
        public void ApplyCustomSignUpCommandValidation()
        {
            RuleFor(x => x.Email)
                .MustAsync(async (email, cancellation) => (await _userService.IsEmailExistAsync(email)))
                .WithMessage(_stringLocalizer[AppLocalizationKeys.EmailIsNotExist]);
        }
        #endregion
    }
}
