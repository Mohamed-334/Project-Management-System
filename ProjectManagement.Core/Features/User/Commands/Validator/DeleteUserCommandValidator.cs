using ProjectManagement.Core.Features.ApplicationUser.Commands.RequestModels;
using ProjectManagement.Core.Features.Roles.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.Service;
using ProjectManagement.Service.ServiceInterfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Roles.Commands.Validators
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommandRequestQuery>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IUserService _userService;
        #endregion

        #region Constructor
        public DeleteUserCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer, IUserService userService)
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
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
        }
        public void ApplyCustomSignUpCommandValidation()
        {
            RuleFor(x => x.Id)
                .MustAsync(async (Id, cancellation) => (await _userService.IsUserIdExistAsync(Id)))
                .WithMessage(_stringLocalizer[AppLocalizationKeys.NotFound]);
        }
        #endregion
    }

}
