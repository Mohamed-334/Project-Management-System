using ProjectManagement.Core.Features.Tasks.Commands.RequestModels;
using ProjectManagement.Core.Mapping.Shared;
using static ProjectManagement.Domain.Enums.EnumExtensions;
using TaskEntity = ProjectManagement.Domain.Entities.ProjectTask;

namespace ProjectManagement.Core.Mapping.TaskMapping
{
    public partial class TaskMapping
    {
        #region Methods
        public void MapFromAddTaskCommandRequestModelToTaskEntity()
        {
            CreateMap<AddTaskCommandRequestModel, TaskEntity>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                    src.StatusCode.HasValue ? ((TaskStatusEnum)src.StatusCode.Value).SplitEnumName() : null))
                .ForMember(dest => dest.StatusLocalization, opt => opt.MapFrom(src =>
                    src.StatusCode.HasValue ? ((TaskStatusEnum)src.StatusCode.Value).GetDisplayName() : null))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src =>
                    src.PriorityCode.HasValue ? ((TaskPriorityEnum)src.PriorityCode.Value).SplitEnumName() : null))
                .ForMember(dest => dest.PriorityLocalization, opt => opt.MapFrom(src =>
                    src.PriorityCode.HasValue ? ((TaskPriorityEnum)src.PriorityCode.Value).GetDisplayName() : null))
                .AfterMap<MetaMappingDataBasedOnDestination<AddTaskCommandRequestModel, TaskEntity>>();
        }
        #endregion
    }
}
