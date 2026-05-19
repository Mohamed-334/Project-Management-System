using ProjectManagement.Core.Features.Projects.Dto;
using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Projects.Queries.RequestModels
{
    public class GetProjectsListQueryRequestModel : IRequest<Response<List<ProjectFullDataDto>>>
    {
    }
}
