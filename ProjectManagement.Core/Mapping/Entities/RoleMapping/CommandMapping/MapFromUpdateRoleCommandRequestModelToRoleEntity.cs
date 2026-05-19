using ProjectManagement.Core.Features.Roles.Commands.RequestModels;
using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Core.Mapping.RoleMapping
{
    public partial class RoleMapping
    {
        #region Methods
        public void MapFromUpdateRoleCommandRequestModelToRoleEntity()
        {
            CreateMap<UpdateRoleCommandRequestModel, Role>()
            .AfterMap<MetaMappingDataBasedOnDestination<UpdateRoleCommandRequestModel, Role>>();
        }
        #endregion
    }
}
