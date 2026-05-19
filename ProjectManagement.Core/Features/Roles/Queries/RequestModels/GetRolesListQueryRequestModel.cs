using ProjectManagement.Core.Features.Roles.Dto;
using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Roles.Queries.RequestModels
{
    public class GetRolesListQueryRequestModel : IRequest<Response<List<RoleFullDataDto>>>
    {
    }
}
