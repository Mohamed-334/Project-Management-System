using ProjectManagement.Core.Features.Roles.Dto;
using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.ApplicationUser.Queries.RequestModels
{
    public class GetUserRolesQueryRequestModel : IRequest<Response<List<RoleFullDataDto>>>
    {
        public int UserId { get; set; }
    }
}
