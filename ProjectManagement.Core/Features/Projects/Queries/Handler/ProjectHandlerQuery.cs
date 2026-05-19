using AutoMapper;
using ProjectManagement.Core.Features.Projects.Dto;
using ProjectManagement.Core.Features.Projects.Queries.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Projects.Queries.Handler
{
    public class ProjectHandlerQuery : ResponseHandler,
                                       IRequestHandler<GetProjectsListQueryRequestModel, Response<List<ProjectFullDataDto>>>,
                                       IRequestHandler<GetProjectByIdQueryRequestModel, Response<ProjectFullDataDto>>,
                                       IRequestHandler<GetProjectsPaginatedListQueryRequestModel, Response<PaginatedList<ProjectFullDataDto>>>,
                                       IRequestHandler<GetProjectsDropDownQueryRequestModel, Response<List<DropDown>>>
    {
        #region Fields
        private readonly IProjectService _projectService;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _projectMapping;
        #endregion

        #region Constructor
        public ProjectHandlerQuery(IProjectService projectService,
                                   IStringLocalizer<AppLocalization> stringLocalizer,
                                   IHttpContextAccessor httpContextAccessor,
                                   IMapper projectMapping) : base(stringLocalizer)
        {
            _projectService = projectService;
            _stringLocalizer = stringLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _projectMapping = projectMapping;
        }
        #endregion

        #region Methods
        public async Task<Response<List<ProjectFullDataDto>>> Handle(GetProjectsListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var projects = await _projectService.GetAllAsync();
            if (projects == null)
                return NotFound<List<ProjectFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var projectsDto = _projectMapping.Map<List<ProjectFullDataDto>>(projects).ToList();
            return Success(projectsDto, _stringLocalizer[AppLocalizationKeys.Success], new { TotalCount = projectsDto.Count });
        }

        public async Task<Response<ProjectFullDataDto>> Handle(GetProjectByIdQueryRequestModel request, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetByIdAsync(request.Id);
            if (project == null)
                return NotFound<ProjectFullDataDto>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var projectDto = _projectMapping.Map<ProjectFullDataDto>(project);
            return Success(projectDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<PaginatedList<ProjectFullDataDto>>> Handle(GetProjectsPaginatedListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var paginatedList = await _projectService.GetPaginatedListAsync(request.PageNumber, request.PageSize);
            if (paginatedList == null)
                return NotFound<PaginatedList<ProjectFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var projectFullDataDtoList = _projectMapping.Map<List<ProjectFullDataDto>>(paginatedList.Data).ToList();
            var paginatedListDto = PaginatedList<ProjectFullDataDto>.Success(projectFullDataDtoList, paginatedList.TotalCount, paginatedList.CurrentPage, paginatedList.PageSize);
            return Success(paginatedListDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<List<DropDown>>> Handle(GetProjectsDropDownQueryRequestModel request, CancellationToken cancellationToken)
        {
            var projects = await _projectService.GetAllAsync();
            if (projects == null)
                return NotFound<List<DropDown>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var dropDowns = _projectMapping.Map<List<DropDown>>(projects).ToList();
            return Success(dropDowns, _stringLocalizer[AppLocalizationKeys.Success]);
        }
        #endregion
    }
}
