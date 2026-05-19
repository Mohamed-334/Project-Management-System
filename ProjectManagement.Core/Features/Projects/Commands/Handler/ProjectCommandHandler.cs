using AutoMapper;
using ProjectManagement.Core.Features.Projects.Commands.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Projects.Commands.Handler
{
    public class ProjectCommandHandler : ResponseHandler,
                                         IRequestHandler<AddProjectCommandRequestModel, Response<string>>,
                                         IRequestHandler<UpdateProjectCommandRequestModel, Response<string>>,
                                         IRequestHandler<DeleteProjectCommandRequestModel, Response<string>>,
                                         IRequestHandler<SoftDeleteAndActivateProjectCommandRequestModel, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        #endregion

        #region Constructor
        public ProjectCommandHandler(IStringLocalizer<AppLocalization> stringLocalizer,
                                     IMapper mapper,
                                     IProjectService projectService,
                                     IAuthenticatedUserService authenticatedUserService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _projectService = projectService;
            _authenticatedUserService = authenticatedUserService;
        }
        #endregion

        #region Methods
        public async Task<Response<string>> Handle(AddProjectCommandRequestModel request, CancellationToken cancellationToken)
        {
            var project = _mapper.Map<Project>(request);
            var result = await _projectService.AddAsync(project);
            if (result == _stringLocalizer[AppLocalizationKeys.AddFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.AddFailed]);
            return Success("", Meta: new { Id = project.Id });
        }

        public async Task<Response<string>> Handle(UpdateProjectCommandRequestModel request, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetByIdAsync(request.Id);
            if (project == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var projectMapper = _mapper.Map(request, project);
            var result = await _projectService.EditAsync(projectMapper);
            if (result == _stringLocalizer[AppLocalizationKeys.UpdateFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.UpdateFailed]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Updated]);
        }

        public async Task<Response<string>> Handle(DeleteProjectCommandRequestModel request, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetByIdAsync(request.Id);
            if (project == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var result = await _projectService.HardDeleteAsync(project);
            if (result == _stringLocalizer[AppLocalizationKeys.DeletedFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.DeletedFailed]);
            return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
        }

        public async Task<Response<string>> Handle(SoftDeleteAndActivateProjectCommandRequestModel request, CancellationToken cancellationToken)
        {
            var project = await _projectService.GetByIdAsync(request.Id);
            if (project == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            project.IsDeleted = !(project.IsDeleted);
            project.DeletionDate = DateTime.UtcNow;
            project.DeleterName = _authenticatedUserService.GetAuthenticatedUserName();
            var result = await _projectService.EditAsync(project);
            if (result == _stringLocalizer[AppLocalizationKeys.DeletedFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.DeletedFailed]);
            if (project.IsDeleted)
                return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Activated]);
        }
        #endregion
    }
}
