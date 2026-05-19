using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Tasks.Commands.RequestModels
{
    public class SoftDeleteAndActivateTaskCommandRequestModel : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
