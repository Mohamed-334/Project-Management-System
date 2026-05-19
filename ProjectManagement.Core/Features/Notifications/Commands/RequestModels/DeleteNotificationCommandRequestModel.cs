using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Notifications.Commands.RequestModels
{
    public class DeleteNotificationCommandRequestModel : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
