namespace ProjectManagement.Core.Features.Tasks.Dto
{
    public class TaskFullDataDto
    {
        public int Id { get; set; }
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
