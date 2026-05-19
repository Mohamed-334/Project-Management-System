using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Projects.Commands.RequestModels
{
    public class UpdateProjectCommandRequestModel : BaseModelWithName, IRequest<Response<string>>
    {
        public string? Description { get; set; }
        public string? DescriptionLocalization { get; set; }
    }
}
