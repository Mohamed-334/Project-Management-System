using AutoMapper;

namespace ProjectManagement.Core.Mapping.TaskMapping
{
    public partial class TaskMapping : Profile
    {
        #region Constructor
        public TaskMapping()
        {
            MapFromAddTaskCommandRequestModelToTaskEntity();
            MapFromUpdateTaskCommandRequestModelToTaskEntity();
            MappingFromTaskToTaskFullDataDto();
            MappingFromTaskToDropDown();
        }
        #endregion
    }
}
