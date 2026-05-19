using ProjectManagement.Core.Features.Notifications.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Notifications.Commands.Validators
{
    public class AddNotificationCommandValidator : AbstractValidator<AddNotificationCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly INotificationService _notificationService;
        #endregion

        #region Constructor
        public AddNotificationCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer, INotificationService notificationService)
        {
            _stringLocalizer = stringLocalizer;
            _notificationService = notificationService;
            ApplySignUpCommandValidation();
            ApplyCustomSignUpCommandValidation();
        }
        #endregion

        #region Methods

        public void ApplySignUpCommandValidation()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
        }
        public void ApplyCustomSignUpCommandValidation()
        {
        }
        #endregion
    }

}

