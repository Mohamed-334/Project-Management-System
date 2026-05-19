using ProjectManagement.Core.Features.Tasks.Dto;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;

namespace ProjectManagement.Core.Features.Tasks.Queries.RequestModels
{
    public class GetTasksPaginatedListQueryRequestModel : IRequest<Response<PaginatedList<TaskFullDataDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public GetTasksPaginatedListQueryRequestModel(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public GetTasksPaginatedListQueryRequestModel() { }
    }
}
