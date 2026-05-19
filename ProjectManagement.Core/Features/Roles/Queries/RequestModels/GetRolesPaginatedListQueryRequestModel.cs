using ProjectManagement.Core.Features.Roles.Dto;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;

namespace ProjectManagement.Core.Features.Roles.Queries.RequestModels
{
    public class GetRolesPaginatedListQueryRequestModel : IRequest<Response<PaginatedList<RoleFullDataDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public GetRolesPaginatedListQueryRequestModel(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public GetRolesPaginatedListQueryRequestModel() { }
    }
}
