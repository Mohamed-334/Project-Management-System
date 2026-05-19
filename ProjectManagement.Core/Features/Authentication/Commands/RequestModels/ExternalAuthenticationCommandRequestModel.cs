using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Authentication.Commands.RequestModels
{
    public class ExternalAuthenticationCommandRequestModel : IRequest<Response<string>>
    {
    }
}
