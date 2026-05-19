using ProjectManagement.Core.Features.Projects.Dto;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;

namespace ProjectManagement.Core.Features.Projects.Queries.RequestModels
{
    public class GetProjectsPaginatedListQueryRequestModel : IRequest<Response<PaginatedList<ProjectFullDataDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public GetProjectsPaginatedListQueryRequestModel(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public GetProjectsPaginatedListQueryRequestModel() { }
    }
}
