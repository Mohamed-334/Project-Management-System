using ProjectManagement.Core.Features.Tasks.Commands.RequestModels;
using ProjectManagement.Infrastructure.Shared.Localization;
using TaskManagement.Service.ServiceInterfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Tasks.Commands.Validators
{
    public class AddTaskCommandValidator : AbstractValidator<AddTaskCommandRequestModel>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly ITaskService _taskService;
        #endregion

        #region Constructor
        public AddTaskCommandValidator(IStringLocalizer<AppLocalization> stringLocalizer, ITaskService taskService)
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
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
            RuleFor(x => x.StatusCode)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
            RuleFor(x => x.PriorityCode)
                .NotEmpty().WithMessage(_stringLocalizer[AppLocalizationKeys.NotEmpty])
                .NotNull().WithMessage(_stringLocalizer[AppLocalizationKeys.Required]);
        }
        public void ApplyCustomValidation()
        {
        }
        #endregion
    }
}
