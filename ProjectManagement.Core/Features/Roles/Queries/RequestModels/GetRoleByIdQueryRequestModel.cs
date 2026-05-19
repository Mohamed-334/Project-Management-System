using ProjectManagement.Core.Features.Roles.Dto;
using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Roles.Queries.RequestModels
{
    public class GetRoleByIdQueryRequestModel : IRequest<Response<RoleFullDataDto>>
    {
        public int RoleId { get; set; }

    }
}
