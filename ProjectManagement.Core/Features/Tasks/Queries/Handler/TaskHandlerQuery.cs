using AutoMapper;
using ProjectManagement.Core.Features.Tasks.Dto;
using ProjectManagement.Core.Features.Tasks.Queries.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using ProjectManagement.Infrastructure.Shared.Localization;
using TaskManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Tasks.Queries.Handler
{
    public class TaskHandlerQuery : ResponseHandler,
                                    IRequestHandler<GetTasksListQueryRequestModel, Response<List<TaskFullDataDto>>>,
                                    IRequestHandler<GetTaskByIdQueryRequestModel, Response<TaskFullDataDto>>,
                                    IRequestHandler<GetTasksPaginatedListQueryRequestModel, Response<PaginatedList<TaskFullDataDto>>>,
                                    IRequestHandler<GetTasksDropDownQueryRequestModel, Response<List<DropDown>>>,
                                    IRequestHandler<GetProjectTasksPaginatedListQueryRequestModel, Response<PaginatedList<TaskFullDataDto>>>
    {
        #region Fields
        private readonly ITaskService _taskService;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _taskMapping;
        #endregion

        #region Constructor
        public TaskHandlerQuery(ITaskService taskService,
                                IStringLocalizer<AppLocalization> stringLocalizer,
                                IHttpContextAccessor httpContextAccessor,
                                IMapper taskMapping) : base(stringLocalizer)
        {
            _taskService = taskService;
            _stringLocalizer = stringLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _taskMapping = taskMapping;
        }
        #endregion

        #region Methods
        public async Task<Response<List<TaskFullDataDto>>> Handle(GetTasksListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var tasks = await _taskService.GetAllAsync();
            if (tasks == null)
                return NotFound<List<TaskFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var tasksDto = _taskMapping.Map<List<TaskFullDataDto>>(tasks).ToList();
            return Success(tasksDto, _stringLocalizer[AppLocalizationKeys.Success], new { TotalCount = tasksDto.Count });
        }

        public async Task<Response<TaskFullDataDto>> Handle(GetTaskByIdQueryRequestModel request, CancellationToken cancellationToken)
        {
            var task = await _taskService.GetByIdAsync(request.Id);
            if (task == null)
                return NotFound<TaskFullDataDto>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var taskDto = _taskMapping.Map<TaskFullDataDto>(task);
            return Success(taskDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<PaginatedList<TaskFullDataDto>>> Handle(GetTasksPaginatedListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var paginatedList = await _taskService.GetPaginatedListAsync(request.PageNumber, request.PageSize);
            if (paginatedList == null)
                return NotFound<PaginatedList<TaskFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var taskFullDataDtoList = _taskMapping.Map<List<TaskFullDataDto>>(paginatedList.Data).ToList();
            var paginatedListDto = PaginatedList<TaskFullDataDto>.Success(taskFullDataDtoList, paginatedList.TotalCount, paginatedList.CurrentPage, paginatedList.PageSize);
            return Success(paginatedListDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<List<DropDown>>> Handle(GetTasksDropDownQueryRequestModel request, CancellationToken cancellationToken)
        {
            var tasks = await _taskService.GetAllAsync();
            if (tasks == null)
                return NotFound<List<DropDown>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var dropDowns = _taskMapping.Map<List<DropDown>>(tasks).ToList();
            return Success(dropDowns, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<PaginatedList<TaskFullDataDto>>> Handle(GetProjectTasksPaginatedListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var paginatedList = await _taskService.GetProjectTasksPaginatedListAsync(request.ProjectId, request.PageNumber, request.PageSize);
            if (paginatedList == null)
                return NotFound<PaginatedList<TaskFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var taskFullDataDtoList = _taskMapping.Map<List<TaskFullDataDto>>(paginatedList.Data).ToList();
            var paginatedListDto = PaginatedList<TaskFullDataDto>.Success(taskFullDataDtoList, paginatedList.TotalCount, paginatedList.CurrentPage, paginatedList.PageSize);
            return Success(paginatedListDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }
        #endregion
    }
}
