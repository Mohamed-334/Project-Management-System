using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Projects.Commands.RequestModels
{
    public class SoftDeleteAndActivateProjectCommandRequestModel : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
