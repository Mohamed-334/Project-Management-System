using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.ApplicationUser.Commands.RequestModels
{
    public class DeleteUserCommandRequestQuery : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
