using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using TaskEntity = ProjectManagement.Domain.Entities.ProjectTask;

namespace ProjectManagement.Core.Mapping.TaskMapping
{
    public partial class TaskMapping
    {
        #region Methods
        public void MappingFromTaskToDropDown()
        {
            CreateMap<TaskEntity, DropDown>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.NameLocalization, opt => opt.MapFrom(src => src.TitleLocalization))
                .AfterMap<MetaMappingDataBasedOnSource<TaskEntity, DropDown>>();
        }
        #endregion
    }
}
