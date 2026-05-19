using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Notifications.Commands.RequestModels
{
    public class SoftDeleteAndActivateNotificationCommandRequestModel : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
