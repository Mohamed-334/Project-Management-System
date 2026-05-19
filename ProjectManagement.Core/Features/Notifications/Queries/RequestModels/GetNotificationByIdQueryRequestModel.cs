using ProjectManagement.Core.Features.Notifications.Dto;
using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Notifications.Queries.RequestModels
{
    public class GetNotificationByIdQueryRequestModel : IRequest<Response<NotificationFullDataDto>>
    {
        public int Id { get; set; }

    }
}
