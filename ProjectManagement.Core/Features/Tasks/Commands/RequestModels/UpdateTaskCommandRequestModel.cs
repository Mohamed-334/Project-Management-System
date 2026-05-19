using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Tasks.Commands.RequestModels
{
    public class UpdateTaskCommandRequestModel : BaseModel, IRequest<Response<string>>
    {
        public string? Title { get; set; }
        public string? TitleLocalization { get; set; }
        public string? Description { get; set; }
        public string? DescriptionLocalization { get; set; }
        public int? StatusCode { get; set; }
        public int? PriorityCode { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ProjectId { get; set; }
    }
}
