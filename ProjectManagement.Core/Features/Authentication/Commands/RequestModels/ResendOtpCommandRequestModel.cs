using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Authentication.Commands.RequestModels
{
    public class ResendOtpCommandRequestModel : IRequest<Response<string>>
    {
        public string? Email { get; set; }
    }
}
