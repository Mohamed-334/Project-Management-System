using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Roles.Commands.RequestModels
{
    public class UpdateRoleCommandRequestModel : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameLocalization { get; set; }
    }
}
