using ProjectManagement.Core.Features.Tasks.Dto;
using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Tasks.Queries.RequestModels
{
    public class GetTaskByIdQueryRequestModel : IRequest<Response<TaskFullDataDto>>
    {
        public int Id { get; set; }
    }
}
