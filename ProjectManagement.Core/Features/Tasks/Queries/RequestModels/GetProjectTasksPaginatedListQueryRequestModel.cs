using ProjectManagement.Core.Features.Tasks.Dto;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;

namespace ProjectManagement.Core.Features.Tasks.Queries.RequestModels
{
    public class GetProjectTasksPaginatedListQueryRequestModel : IRequest<Response<PaginatedList<TaskFullDataDto>>>
    {
        public int ProjectId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
