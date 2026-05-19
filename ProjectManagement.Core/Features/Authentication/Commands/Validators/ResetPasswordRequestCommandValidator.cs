using ProjectManagement.Core.Features.Authentication.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Authentication.Commands.Validators
{
    public class ResetPasswordRequestCommandValidator : AbstractValidator<ResetPasswordRequestCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        #endregion

        #region Constructor
        public ResetPasswordRequestCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer)
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
        }
        #endregion
    }
}
