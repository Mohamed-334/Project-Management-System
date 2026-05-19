using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;

namespace ProjectManagement.Core.Mapping.ProjectMapping
{
    public partial class ProjectMapping
    {
        #region Methods
        public void MappingFromProjectToDropDown()
        {
            CreateMap<Project, DropDown>()
                .AfterMap<MetaMappingDataBasedOnSource<Project, DropDown>>();
        }
        #endregion
    }
}
