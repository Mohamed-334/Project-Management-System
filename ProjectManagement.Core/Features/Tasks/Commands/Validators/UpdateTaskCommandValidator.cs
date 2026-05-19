using ProjectManagement.Core.Features.Tasks.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using TaskManagement.Service.ServiceInterfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Tasks.Commands.Validators
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly ITaskService _taskService;
        #endregion

        #region Constructor
        public UpdateTaskCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer, ITaskService taskService)
        {
            _stringLocalizer = stringLocalizer;
            _taskService = taskService;
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
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
            RuleFor(x => x.StatusCode)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
        }
        public void ApplyCustomValidation()
        {
        }
        #endregion
    }
}
