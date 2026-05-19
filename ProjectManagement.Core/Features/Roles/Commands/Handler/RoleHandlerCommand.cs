using AutoMapper;
using ProjectManagement.Core.Features.Roles.Commands.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Authentication.Commands.Handlers
{
    public class RoleHandlerCommand : ResponseHandler,
                                        IRequestHandler<AddRoleCommandRequestModel, Response<string>>,
                                        IRequestHandler<UpdateRoleCommandRequestModel, Response<string>>,
                                        IRequestHandler<DeleteRoleCommandRequestModel, Response<string>>,
                                        IRequestHandler<SoftDeleteAndActivateRoleCommandRequestQuery, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRoleService _roleService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        #endregion

        #region Constructor
        public RoleHandlerCommand(IStringLocalizer<AppLocalization> stringLocalizer, IMapper mapper, IAuthenticationService authenticationService, IRoleService roleService, IAuthenticatedUserService authenticatedUserService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _roleService = roleService;
            _authenticatedUserService = authenticatedUserService;
        }
        #endregion

        #region Methods
        public async Task<Response<string>> Handle(AddRoleCommandRequestModel request, CancellationToken cancellationToken)
        {
            var Role = _mapper.Map<Role>(request);
            var result = await _roleService.AddRoleAsync(Role);
            if (!result.Succeeded)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.AddFailed]);
            return Success<string>(Meta: new { Id = Role.Id });
        }

        public async Task<Response<string>> Handle(UpdateRoleCommandRequestModel request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetByIdAsync(request.Id);
            if (role == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);

            var roleMapper = _mapper.Map(request, role);
            var result = await _roleService.EditAsync(roleMapper);
            if (!result.Succeeded)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.UpdateFailed]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Updated]);
        }

        public async Task<Response<string>> Handle(DeleteRoleCommandRequestModel request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetByIdAsync(request.Id);
            if (role == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var result = await _roleService.HardDeleteAsync(role);
            if (!result.Succeeded)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToRemoveOldRoles]);
            return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
        }
        public async Task<Response<string>> Handle(SoftDeleteAndActivateRoleCommandRequestQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetByIdAsync(request.Id);
            if (role == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            role.IsDeleted = !(role.IsDeleted);
            role.DeletionDate = DateTime.UtcNow;
            role.DeleterName = _authenticatedUserService.GetAuthenticatedUserName();
            var result = await _roleService.EditAsync(role);

            if (!result.Succeeded)
                return Deleted<string>(_stringLocalizer[AppLocalizationKeys.DeletedFailed]);
            if (role.IsDeleted)
                return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Activated]);
        }
        #endregion
    }
}
