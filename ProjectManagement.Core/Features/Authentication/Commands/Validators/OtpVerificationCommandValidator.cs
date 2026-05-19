using ProjectManagement.Core.Features.Authentication.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Authentication.Commands.Validators
{
    public class OtpVerificationCommandValidator : AbstractValidator<OtpVerificationCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        #endregion

        #region Constructor
        public OtpVerificationCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer)
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
            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
        }
        #endregion
    }
}
