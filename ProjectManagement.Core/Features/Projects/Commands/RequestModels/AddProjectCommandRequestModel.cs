using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Projects.Commands.RequestModels
{
    public class AddProjectCommandRequestModel : IRequest<Response<string>>
    {
        public string? Name { get; set; }
        public string? NameLocalization { get; set; }
        public string? Description { get; set; }
        public string? DescriptionLocalization { get; set; }
    }
}
