using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Authentication.Commands.RequestModels
{
    public class LogoutCommandRequestModel : IRequest<Response<string>>
    {
    }
}
