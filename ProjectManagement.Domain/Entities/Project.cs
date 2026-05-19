using ProjectManagement.Domain.Shared.BaseEntity.Implementations;

namespace ProjectManagement.Domain.Entities
{
    public class Project : BaseEntityWithName
    {
        public string? Description { get; set; }
        public string? DescriptionLocalization { get; set; }
        public virtual ICollection<ProjectTask>? Tasks { get; set; }
    }
}
