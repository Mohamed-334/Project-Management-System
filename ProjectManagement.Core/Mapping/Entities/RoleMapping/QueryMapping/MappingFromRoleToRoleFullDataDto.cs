using ProjectManagement.Core.Features.Roles.Dto;
using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Core.Mapping.RoleMapping
{
    public partial class RoleMapping
    {
        #region Methods
        public void MappingFromRoleToRoleFullDataDto()
        {
            CreateMap<Role, RoleFullDataDto>()
                .AfterMap<MetaMappingDataBasedOnSource<Role, RoleFullDataDto>>();
        }
        #endregion
    }

}
