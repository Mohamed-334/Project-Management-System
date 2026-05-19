using ProjectManagement.Core.Features.Authentication.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Authentication.Commands.Validators
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        #endregion

        #region Constructor
        public ResetPasswordCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            ApplySignUpCommandValidation();
        }
        #endregion

        #region Methods

        public void ApplySignUpCommandValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required])
                .Equal(x => x.Password).WithMessage(_stringLocalizer[AppLocalizationKeys.PasswordNotEqualConfirmPass]);

        }
        #endregion
    }
}
