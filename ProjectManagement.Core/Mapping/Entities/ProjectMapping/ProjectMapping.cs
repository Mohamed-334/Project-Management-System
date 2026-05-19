using AutoMapper;

namespace ProjectManagement.Core.Mapping.ProjectMapping
{
    public partial class ProjectMapping : Profile
    {
        #region Constructor
        public ProjectMapping()
        {
            MapFromAddProjectCommandRequestModelToProjectEntity();
            MapFromUpdateProjectCommandRequestModelToProjectEntity();
            MappingFromProjectToProjectFullDataDto();
            MappingFromProjectToDropDown();
        }
        #endregion
    }
}
