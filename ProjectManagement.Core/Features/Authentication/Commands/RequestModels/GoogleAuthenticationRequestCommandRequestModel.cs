using ProjectManagement.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace ProjectManagement.Core.Features.Authentication.Commands.RequestModels
{
    public class GoogleAuthenticationRequestCommandRequestModel : IRequest<Response<AuthenticationProperties>>
    {
        public string? RedirectUrl { get; set; }
    }
}
