using ProjectManagement.Core.Features.Projects.Commands.RequestModels;
using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Core.Mapping.ProjectMapping
{
    public partial class ProjectMapping
    {
        #region Methods
        public void MapFromAddProjectCommandRequestModelToProjectEntity()
        {
            CreateMap<AddProjectCommandRequestModel, Project>()
                .AfterMap<MetaMappingDataBasedOnDestination<AddProjectCommandRequestModel, Project>>();
        }
        #endregion
    }
}
