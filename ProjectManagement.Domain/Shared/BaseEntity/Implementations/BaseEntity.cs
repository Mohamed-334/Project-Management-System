using ProjectManagement.Domain.Shared.BaseEntity.Interfaces;

namespace ProjectManagement.Domain.Shared.BaseEntity.Implementations
{
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? CreatorName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? ModifierName { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string? DeleterName { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}
