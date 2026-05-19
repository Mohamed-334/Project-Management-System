using AutoMapper;
using ProjectManagement.Core.Features.Tasks.Commands.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Infrastructure.Shared.Localization;
using TaskManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.ServiceInterfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using TaskEntity = ProjectManagement.Domain.Entities.ProjectTask;

namespace ProjectManagement.Core.Features.Tasks.Commands.Handler
{
    public class TaskCommandHandler : ResponseHandler,
                                      IRequestHandler<AddTaskCommandRequestModel, Response<string>>,
                                      IRequestHandler<UpdateTaskCommandRequestModel, Response<string>>,
                                      IRequestHandler<DeleteTaskCommandRequestModel, Response<string>>,
                                      IRequestHandler<SoftDeleteAndActivateTaskCommandRequestModel, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        #endregion

        #region Constructor
        public TaskCommandHandler(IStringLocalizer<AppLocalization> stringLocalizer,
                                  IMapper mapper,
                                  ITaskService taskService,
                                  IAuthenticatedUserService authenticatedUserService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _taskService = taskService;
            _authenticatedUserService = authenticatedUserService;
        }
        #endregion

        #region Methods
        public async Task<Response<string>> Handle(AddTaskCommandRequestModel request, CancellationToken cancellationToken)
        {
            var task = _mapper.Map<TaskEntity>(request);
            var result = await _taskService.AddAsync(task);
            if (result == _stringLocalizer[AppLocalizationKeys.AddFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.AddFailed]);
            return Success("", Meta: new { Id = task.Id });
        }

        public async Task<Response<string>> Handle(UpdateTaskCommandRequestModel request, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetByIdAsync(request.Id);
            if (task == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var taskMapper = _mapper.Map(request, task);
            var result = await _taskService.EditAsync(taskMapper);
            if (result == _stringLocalizer[AppLocalizationKeys.UpdateFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.UpdateFailed]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Updated]);
        }

        public async Task<Response<string>> Handle(DeleteTaskCommandRequestModel request, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetByIdAsync(request.Id);
            if (task == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var result = await _taskService.HardDeleteAsync(task);
            if (result == _stringLocalizer[AppLocalizationKeys.DeletedFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.DeletedFailed]);
            return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
        }

        public async Task<Response<string>> Handle(SoftDeleteAndActivateTaskCommandRequestModel request, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetByIdAsync(request.Id);
            if (task == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            task.IsDeleted = !(task.IsDeleted);
            task.DeletionDate = DateTime.UtcNow;
            task.DeleterName = _authenticatedUserService.GetAuthenticatedUserName();
            var result = await _taskService.EditAsync(task);
            if (result == _stringLocalizer[AppLocalizationKeys.DeletedFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.DeletedFailed]);
            if (task.IsDeleted)
                return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Activated]);
        }
        #endregion
    }
}
