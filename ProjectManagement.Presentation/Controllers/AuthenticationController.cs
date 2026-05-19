using ProjectManagement.Core.Features.Authentication.Commands.RequestModels;
using ProjectManagement.Domain.Meta;
using ProjectManagement.Presentation.Shared.BaseController;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagement.Presentation.Controllers
{
    [ApiController]
    public class AuthenticationController : BaseControllerApp
    {
        [HttpGet(Router.AuthenticationRouting.GoogleAuthenticationRequest)]
        public async Task<IActionResult> GoogleAuthenticationRequest()
        {
            var redirectUrl = Url.Action(nameof(ExternalAuthentication), "Authentication", new { provider = "Google" });
            var response = await _mediator.Send(new GoogleAuthenticationRequestCommandRequestModel
            {
                RedirectUrl = redirectUrl
            });
            return Challenge(response?.Data ?? new AuthenticationProperties(), GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet(Router.AuthenticationRouting.LinkedInAuthenticationRequest)]
        public async Task<IActionResult> LinkedInAuthenticationRequest()
        {
            var redirectUrl = Url.Action(nameof(ExternalAuthentication), "Authentication", new { provider = "LinkedIn" });
            var response = await _mediator.Send(new LinkedInAuthenticationRequestCommandRequestModel
            {
                RedirectUrl = redirectUrl
            });
            return Challenge(response.Data ?? new AuthenticationProperties(), "LinkedIn");
        }
        [HttpGet(Router.AuthenticationRouting.ExternalAuthentication)]
        public async Task<IActionResult> ExternalAuthentication([FromQuery] string provider)
        {
            var response = await _mediator.Send(new ExternalAuthenticationCommandRequestModel());
            return Result(response);
        }
        [HttpPost(Router.AuthenticationRouting.SignUp)]
        public async Task<IActionResult> SignUp([FromForm] SignUpCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.AuthenticationRouting.SignIn)]
        public async Task<IActionResult> SignIn([FromBody] SignInCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.AuthenticationRouting.ResendOtp)]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.AuthenticationRouting.VerifyRegistrationOtp)]
        public async Task<IActionResult> VerifyRegistrationOtp([FromBody] OtpVerificationCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.AuthenticationRouting.VerifyResetPasswordOtp)]
        public async Task<IActionResult> VerifyResetPasswordOtp([FromBody] ResetPasswordOtpVerificationCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPut(Router.AuthenticationRouting.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.AuthenticationRouting.ResetPasswordRequest)]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequestCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.AuthenticationRouting.ResetPassword)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.AuthenticationRouting.Logout)]
        public async Task<IActionResult> Logout()
        {
            var response = await _mediator.Send(new LogoutCommandRequestModel());
            return Result(response);
        }
    }
}
