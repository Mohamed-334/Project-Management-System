using AutoMapper;
using ProjectManagement.Core.Features.ApplicationUser.Commands.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.ApplicationUser.Commands.Handler
{
    public class UserHandlerCommand : ResponseHandler,
                                      IRequestHandler<UpdateUserCommandRequestQuery, Response<string>>,
                                      IRequestHandler<DeleteUserCommandRequestQuery, Response<string>>,
                                      IRequestHandler<SoftDeleteAndActivateUserCommandRequestQuery, Response<string>>
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        #endregion

        #region Constructor
        public UserHandlerCommand(IMapper mapper, IStringLocalizer<AppLocalization> stringLocalizer, IUserService userService, IAuthenticatedUserService authenticatedUserService, IRoleService roleService) : base(stringLocalizer)
        {
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userService = userService;
            _authenticatedUserService = authenticatedUserService;
            _roleService = roleService;
        }
        #endregion

        #region Methods
        public async Task<Response<string>> Handle(UpdateUserCommandRequestQuery request, CancellationToken cancellationToken)
        {
            var OldUser = await _userService.GetByIdAsync(request.Id);
            if (OldUser == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);

            var UploadImageResult = await _userService.UploadFileAsync("Users", request?.ProfileImageFile!);
            if (UploadImageResult == _stringLocalizer[AppLocalizationKeys.FailedToUploadImage])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToUploadImage]);
            request!.ProfileImageUrl = UploadImageResult;

            var NewUser = _mapper.Map(request, OldUser);
            var result = await _userService.EditAsync(NewUser);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new ValidationFailure
                {
                    ErrorMessage = e.Description,
                    ErrorCode = e.Code

                }).ToList();
                throw new ValidationException(_stringLocalizer[AppLocalizationKeys.UpdateFailed], errors);
            }

            if (request.RoleId != null && request.RoleId != 0)
            {

                var userRoles = await _userService.GetUserRolesAsync(OldUser);
                var removeRolesResult = await _userService.RemoveRolesAsync(OldUser, userRoles);
                if (!removeRolesResult.Succeeded)
                    return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToRemoveOldRoles]);

                var NewRole = await _roleService.GetByIdAsync(request.RoleId.Value);

                if (NewRole == null)
                    return NotFound<string>(_stringLocalizer[AppLocalizationKeys.RoleNotExist]);
                var addToRoleResult = await _userService.AddUserToRoleAsync(NewUser, NewRole?.Name!);
                if (!addToRoleResult.Succeeded)
                    return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToAddNewRoles]);
            }
            return Success<string>();

        }

        public async Task<Response<string>> Handle(DeleteUserCommandRequestQuery request, CancellationToken cancellationToken)
        {
            var User = await _userService.GetByIdAsync(request.Id);
            if (User == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);

            var result = await _userService.HardDeleteAsync(User);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new ValidationFailure
                {
                    ErrorMessage = e.Description,
                    ErrorCode = e.Code

                }).ToList();
                throw new ValidationException(_stringLocalizer[AppLocalizationKeys.DeletedFailed], errors);
            }

            return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
        }

        public async Task<Response<string>> Handle(SoftDeleteAndActivateUserCommandRequestQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(request.Id);
            if (user == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            user.IsDeleted = !(user.IsDeleted);
            user.DeletionDate = DateTime.UtcNow;
            user.DeleterName = _authenticatedUserService.GetAuthenticatedUserName();
            var result = await _userService.EditAsync(user);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new ValidationFailure
                {
                    ErrorMessage = e.Description,
                    ErrorCode = e.Code

                }).ToList();
                throw new ValidationException(_stringLocalizer[AppLocalizationKeys.DeletedFailed], errors);
            }   
            if (user.IsDeleted)
                return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Activated]);
        }

        #endregion
    }
}

