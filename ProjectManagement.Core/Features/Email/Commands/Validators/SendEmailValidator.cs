using ProjectManagement.Core.Features.Email.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Email.Commands.Validators
{
    internal class SendEmailValidator : AbstractValidator<SendEmailCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _localizer;
        #endregion
        #region Constructors
        public SendEmailValidator(IStringLocalizer<AppLocalization> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }
        #endregion
        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage(_localizer[AppLocalizationKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[AppLocalizationKeys.Required]);

            RuleFor(x => x.Message)
                 .NotEmpty().WithMessage(_localizer[AppLocalizationKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[AppLocalizationKeys.Required]);
        }
        #endregion
    }
}
