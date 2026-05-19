using ProjectManagement.Core.Features.Tasks.Dto;
using ProjectManagement.Core.Mapping.Shared;
using TaskEntity = ProjectManagement.Domain.Entities.ProjectTask;

namespace ProjectManagement.Core.Mapping.TaskMapping
{
    public partial class TaskMapping
    {
        #region Methods
        public void MappingFromTaskToTaskFullDataDto()
        {
            CreateMap<TaskEntity, TaskFullDataDto>()
                .AfterMap<MetaMappingDataBasedOnSource<TaskEntity, TaskFullDataDto>>();
        }
        #endregion
    }
}
