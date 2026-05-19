using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Shared.BaseFeatures.Crud.Commands.RequestModels
{
    public class DeleteCommandValidator : AbstractValidator<DeleteCommandRequestModelQuery>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        #endregion

        #region Constructor
        public DeleteCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer, IRoleService roleService)
        {
            _stringLocalizer = stringLocalizer;
            ApplySignUpCommandValidation();
            ApplyCustomSignUpCommandValidation();
        }
        #endregion

        #region Methods

        public void ApplySignUpCommandValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
        }
        public void ApplyCustomSignUpCommandValidation()
        {
        }
        #endregion
    }

}
