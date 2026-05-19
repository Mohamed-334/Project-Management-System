using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Email.Commands.RequestModels
{
    public class SendEmailCommandRequestModel : IRequest<Response<string>>
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
