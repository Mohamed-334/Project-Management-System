
using ProjectManagement.Core.Features.ApplicationUser.DTO;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;

namespace ProjectManagement.Core.Features.ApplicationUser.Queries.RequestModels
{
    public class GetUsersPaginatedListQueryRequestModel : IRequest<Response<PaginatedList<UserFullDataDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public GetUsersPaginatedListQueryRequestModel(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public GetUsersPaginatedListQueryRequestModel() { }
    }
}
