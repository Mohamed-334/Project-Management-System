using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Roles.Commands.RequestModels
{
    public class DeleteRoleCommandRequestModel : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
