using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Domain.Entities
{
    public class ProjectTask :BaseEntity
    {
        public string? Title { get; set; }
        public string? TitleLocalization { get; set; }
        public string? Description { get; set; }
        public string? DescriptionLocalization { get; set; }
        public string? Status { get; set; }
        public string? StatusLocalization { get; set; }
        public int? StatusCode { get; set; }
        public DateTime? DueDate { get; set; }
        public int? PriorityCode { get; set; }
        public string? Priority { get; set; }
        public string? PriorityLocalization { get; set; }
        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
