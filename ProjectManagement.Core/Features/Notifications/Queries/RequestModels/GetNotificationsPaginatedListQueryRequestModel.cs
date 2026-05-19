using ProjectManagement.Core.Features.Notifications.Dto;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;

namespace ProjectManagement.Core.Features.Notifications.Queries.RequestModels
{
    public class GetNotificationsPaginatedListQueryRequestModel : IRequest<Response<PaginatedList<NotificationFullDataDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public GetNotificationsPaginatedListQueryRequestModel(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public GetNotificationsPaginatedListQueryRequestModel() { }
    }
}
