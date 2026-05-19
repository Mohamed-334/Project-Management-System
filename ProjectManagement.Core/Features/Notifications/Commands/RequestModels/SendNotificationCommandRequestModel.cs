using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Notifications.Commands.RequestModels
{
    public class SendNotificationCommandRequestModel : IRequest<Response<string>>
    {
        public string? Title { get; set; }
        public string? TitleLocalized { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public List<int> Users { get; set; } = new List<int>();
    }
}
