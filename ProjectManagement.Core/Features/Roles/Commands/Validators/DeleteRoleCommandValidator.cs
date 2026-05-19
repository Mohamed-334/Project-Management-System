using ProjectManagement.Core.Features.Roles.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Roles.Commands.Validators
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IRoleService _roleService;
        #endregion

        #region Constructor
        public DeleteRoleCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer, IRoleService roleService)
        {
            _stringLocalizer = stringLocalizer;
            _roleService = roleService;
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
