using ProjectManagement.Core.Features.Projects.Dto;
using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Projects.Queries.RequestModels
{
    public class GetProjectByIdQueryRequestModel : IRequest<Response<ProjectFullDataDto>>
    {
        public int Id { get; set; }
    }
}
