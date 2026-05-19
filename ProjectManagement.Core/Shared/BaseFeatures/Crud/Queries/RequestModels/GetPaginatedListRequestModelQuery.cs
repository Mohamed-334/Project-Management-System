using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;

namespace ProjectManagement.Core.Shared.BaseFeatures.Crud.Queries.RequestModels
{
    public class GetPaginatedListRequestModelQuery<TDto> : IRequest<Response<PaginatedList<TDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPaginatedListRequestModelQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
