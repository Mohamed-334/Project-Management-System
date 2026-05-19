using ProjectManagement.Core.Features.Roles.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Roles.Commands.Validators
{
    public class AddRoleCommandValidator : AbstractValidator<AddRoleCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IRoleService _roleService;
        #endregion

        #region Constructor
        public AddRoleCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer, IRoleService roleService)
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
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
            RuleFor(x => x.NameLocalization)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
        }
        public void ApplyCustomSignUpCommandValidation()
        {
            RuleFor(x => x.Name)
                .MustAsync(async (RoleName, cancellation) => !(await _roleService.IsRoleNameExistAsync(RoleName)))
                .WithMessage(_stringLocalizer[AppLocalizationKeys.UserNameIsExist]);
        }
        #endregion
    }

}
