using ProjectManagement.Core.Features.Projects.Dto;
using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Core.Mapping.ProjectMapping
{
    public partial class ProjectMapping
    {
        #region Methods
        public void MappingFromProjectToProjectFullDataDto()
        {
            CreateMap<Project, ProjectFullDataDto>()
                .AfterMap<MetaMappingDataBasedOnSource<Project, ProjectFullDataDto>>();
        }
        #endregion
    }
}
