using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using MediatR;

namespace ProjectManagement.Core.Features.Projects.Queries.RequestModels
{
    public class GetProjectsDropDownQueryRequestModel : IRequest<Response<List<DropDown>>>
    {
    }
}
