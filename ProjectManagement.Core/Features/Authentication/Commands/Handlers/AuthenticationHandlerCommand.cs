using AutoMapper;
using Azure.Core;
using ProjectManagement.Core.Features.Authentication.Commands.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using static ProjectManagement.Domain.Enums.EnumExtensions;
using IAuthenticationService = ProjectManagement.Service.ServiceInterfaces.IAuthenticationService;

namespace ProjectManagement.Core.Features.Authentication.Commands.Handlers
{
    public class AuthenticationHandlerCommand : ResponseHandler,
                                        IRequestHandler<SignUpCommandRequestModel, Response<string>>,
                                        IRequestHandler<ChangePasswordCommandRequestModel, Response<string>>,
                                        IRequestHandler<SignInCommandRequestModel, Response<string>>,
                                        IRequestHandler<OtpVerificationCommandRequestModel, Response<string>>,
                                        IRequestHandler<GoogleAuthenticationRequestCommandRequestModel, Response<AuthenticationProperties>>,
                                        IRequestHandler<LinkedInAuthenticationRequestCommandRequestModel, Response<AuthenticationProperties>>,
                                        IRequestHandler<ExternalAuthenticationCommandRequestModel, Response<string>>,
                                        IRequestHandler<ResetPasswordRequestCommandRequestModel, Response<string>>,
                                        IRequestHandler<ResetPasswordOtpVerificationCommandRequestModel, Response<string>>,
                                        IRequestHandler<ResetPasswordCommandRequestModel, Response<string>>,
                                        IRequestHandler<ResendOtpCommandRequestModel, Response<string>>,
                                        IRequestHandler<LogoutCommandRequestModel, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public AuthenticationHandlerCommand(IStringLocalizer<AppLocalization> stringLocalizer, IMapper mapper, IAuthenticationService authenticationService, IUserService userService, IEmailService emailService, IFileService fileService, IHttpContextAccessor httpContextAccessor, IRoleService roleService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _userService = userService;
            _emailService = emailService;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
        }
        #endregion

        #region Methods
        public async Task<Response<string>> Handle(SignUpCommandRequestModel request, CancellationToken cancellationToken)
        {
            var User = _mapper.Map<User>(request);

            var UploadImageResult = await _userService.UploadFileAsync("Users", request?.ProfileImageFile!);
            if (UploadImageResult == _stringLocalizer[AppLocalizationKeys.FailedToUploadImage])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToUploadImage]);
            request!.ProfileImageUrl = UploadImageResult;

            var result = await _authenticationService.SignUpAsync(User, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest<string>(result.Errors.FirstOrDefault()?.Description);
            }
            // Handle Role Assignment
            request.RoleId = request.RoleId != null ? request.RoleId : 2;
            if (request?.RoleId! != null && request.RoleId != 0)
            {
                var requestRole = await _roleService.GetByIdAsync(request.RoleId.Value);
                if (await _userService.IsUserInRoleAsync(User, requestRole?.Name!))
                    return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.RoleIsUsed]);
                var RoleResult = await _userService.AddUserToRoleAsync(User, requestRole?.Name!);

                if (!RoleResult.Succeeded)
                    return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToAddNewRoles]);
            }

            var SendOtpResult = await _authenticationService.SendOtpAsync(User, OtpReasonEnum.Register);
            if (SendOtpResult != _stringLocalizer[AppLocalizationKeys.Success])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToGenerateOtp]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.OtpGenerated], Meta: new { Id = User.Id });
        }

        public async Task<Response<string>> Handle(ChangePasswordCommandRequestModel request, CancellationToken cancellationToken)
        {
            var User = await _userService.GetUserByEmailAsync(request.Email);
            if (User == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var result = await _authenticationService.ChangePasswordAsync(User, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new ValidationFailure
                {
                    ErrorMessage = e.Description,
                    ErrorCode = e.Code
                }).ToList();
                throw new ValidationException(_stringLocalizer[AppLocalizationKeys.ChangePassFailed], errors);
            }
            return Success<string>();
        }
        public async Task<Response<string>> Handle(SignInCommandRequestModel request, CancellationToken cancellationToken)
        {
            var User = await _userService.GetUserByEmailAsync(request.Email);
            if (User == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);

            var PasswordCheck = await _authenticationService.CheckSignInPasswordAsync(User, request.Password, false);
            if (!PasswordCheck.Succeeded)
                return UnprocessableEntity<string>(_stringLocalizer[AppLocalizationKeys.PasswordNotCorrect]);

            if (!User.EmailConfirmed)
            {
                var SendOtpResult = await _authenticationService.SendOtpAsync(User, OtpReasonEnum.Register);
                if (SendOtpResult != _stringLocalizer[AppLocalizationKeys.Success])
                    return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToGenerateOtp]);
                return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.OtpGenerated]);
            }

            var AccessToken = await _authenticationService.SetTokenInCookieAsync(User);
            var meta = new
            {
                Id = User.Id,
                Name = User.Name,
                Email = User.Email,
                Role = User.UserRoles?.FirstOrDefault()?.Role?.Name
            };
            return Success<string>(entity: AccessToken, msg: _stringLocalizer[AppLocalizationKeys.LoginSuccessfully],Meta: meta);
        }

        public async Task<Response<string>> Handle(OtpVerificationCommandRequestModel request, CancellationToken cancellationToken)
        {
            var User = await _userService.GetUserByEmailAsync(request.Email);
            if (User == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var result = await _authenticationService.VerifyOtpAsync(User, request.OtpCode);
            if (!result)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.FailedToVerifyOtp]);
            User.TwoFactorEnabled = true;
            User.EmailConfirmed = true;
            await _userService.EditAsync(User);
            var AccessToken = await _authenticationService.SetTokenInCookieAsync(User);

            var meta = new
            {
                Id = User.Id,
                Name = User.Name,
                Email = User.Email,
                Role = User.UserRoles?.FirstOrDefault()?.Role?.Name
            };
            return Success<string>(entity: AccessToken, msg: _stringLocalizer[AppLocalizationKeys.OtpVerified], Meta: meta);
        }

        public async Task<Response<string>> Handle(ResetPasswordRequestCommandRequestModel request, CancellationToken cancellationToken)
        {
            var User = await _userService.GetUserByEmailAsync(request.Email);
            if (User == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);

            var SendOtpResult = await _authenticationService.SendOtpAsync(User, OtpReasonEnum.ResetPassword);
            if (SendOtpResult != _stringLocalizer[AppLocalizationKeys.Success])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToGenerateOtp]);

            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.OtpGenerated]);

        }

        public async Task<Response<string>> Handle(ResetPasswordOtpVerificationCommandRequestModel request, CancellationToken cancellationToken)
        {
            var User = await _userService.GetUserByEmailAsync(request.Email);
            if (User == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var result = await _authenticationService.VerifyOtpAsync(User, request.OtpCode);
            if (!result)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.FailedToVerifyOtp]);

            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.OtpVerified]);
        }

        public async Task<Response<string>> Handle(ResetPasswordCommandRequestModel request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var result = await _authenticationService.ResetPasswordAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.ChangePassFailed]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.PasswordChanged]);
        }

        public async Task<Response<string>> Handle(ResendOtpCommandRequestModel request, CancellationToken cancellationToken)
        {
            var User = await _userService.GetUserByEmailAsync(request.Email!);
            if (User == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);

            var SendOtpResult = await _authenticationService.SendOtpAsync(User, OtpReasonEnum.Resend);
            if (SendOtpResult != _stringLocalizer[AppLocalizationKeys.Success])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToGenerateOtp]);

            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.OtpGenerated]);
        }
        public async Task<Response<string>> Handle(LogoutCommandRequestModel request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.Logout();
            if (result != _stringLocalizer[AppLocalizationKeys.Success])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToLogout]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Logout]);
        }
        public async Task<Response<AuthenticationProperties>> Handle(GoogleAuthenticationRequestCommandRequestModel request, CancellationToken cancellationToken)
        {
            var properties = _authenticationService.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, request.RedirectUrl!);
            return Success(properties);
        }
        public async Task<Response<AuthenticationProperties>> Handle(LinkedInAuthenticationRequestCommandRequestModel request, CancellationToken cancellationToken)
        {
            var properties = _authenticationService.ConfigureExternalAuthenticationProperties("LinkedIn", request.RedirectUrl!);
            return Success(properties);
        }

        public async Task<Response<string>> Handle(ExternalAuthenticationCommandRequestModel request, CancellationToken cancellationToken)
        {
            var info = await _authenticationService.GetExternalAuthenticationInfoAsync();
            if (info == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.BadRequest]);

            var signInResult = await _authenticationService.ExternalAuthenticationSignInAsync(info.LoginProvider, info.ProviderKey);
            if (!signInResult.Succeeded)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                if (string.IsNullOrEmpty(email))
                    return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.EmailIsNotExist]);

                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = email,
                        Email = email,
                        Name = name,
                        EmailConfirmed = true,
                        TwoFactorEnabled = false,
                    };

                    var createResult = await _authenticationService.ExternalSignUpAsync(user);
                    if (!createResult.Succeeded)
                        return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToAddUser]);

                    var requestRole = await _roleService.GetByIdAsync(3);
                    if (await _userService.IsUserInRoleAsync(user, requestRole?.Name!))
                        return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.RoleIsUsed]);

                    var roleResult = await _userService.AddUserToRoleAsync(user, requestRole?.Name!);
                    if (!roleResult.Succeeded)
                        return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToAddNewRoles]);
                }

                var existingLogins = await _authenticationService.GetLoginsAsync(user);
                bool alreadyLinked = existingLogins.Any(l =>
                    l.LoginProvider == info.LoginProvider &&
                    l.ProviderKey == info.ProviderKey);

                if (!alreadyLinked)
                {
                    var addLoginResult = await _authenticationService.AddExternalLoginAsync(user, info);
                    if (!addLoginResult.Succeeded)
                        return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToLogin]);
                }
            }

            var existingUser = await _authenticationService.FindByLoginAsync(info);
            if (existingUser == null)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                existingUser = await _userService.GetUserByEmailAsync(email ?? "");
            }

            if (existingUser == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.UserIsNotFound]);

            var token = await _authenticationService.SetTokenInCookieAsync(existingUser);
            return Success(entity: token, msg: _stringLocalizer[AppLocalizationKeys.LoginSuccessfully], Meta: new
            {
                Id = existingUser?.Id,
                Role = existingUser?.UserRoles?.FirstOrDefault()?.Role?.Name
            });
        }
        #endregion
    }
}
