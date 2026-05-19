using ProjectManagement.Core.Features.Projects.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Projects.Commands.Validators
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IProjectService _projectService;
        #endregion

        #region Constructor
        public UpdateProjectCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer, IProjectService projectService)
        {
            _stringLocalizer = stringLocalizer;
            _projectService = projectService;
            ApplyValidation();
            ApplyCustomValidation();
        }
        #endregion

        #region Methods
        public void ApplyValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
        }
        public void ApplyCustomValidation()
        {
        }
        #endregion
    }
}
