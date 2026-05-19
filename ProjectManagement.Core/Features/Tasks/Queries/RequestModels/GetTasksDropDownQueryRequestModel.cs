using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using MediatR;

namespace ProjectManagement.Core.Features.Tasks.Queries.RequestModels
{
    public class GetTasksDropDownQueryRequestModel : IRequest<Response<List<DropDown>>>
    {
    }
}
