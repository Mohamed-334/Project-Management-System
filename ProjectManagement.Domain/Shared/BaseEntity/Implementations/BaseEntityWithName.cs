using ProjectManagement.Domain.Shared.BaseEntity.Interfaces;

namespace ProjectManagement.Domain.Shared.BaseEntity.Implementations
{
    public class BaseEntityWithName : BaseEntity, IBaseEntityWithName
    {
        public string? Name { get; set; }
        public string? NameLocalization { get; set; }
    }
}
