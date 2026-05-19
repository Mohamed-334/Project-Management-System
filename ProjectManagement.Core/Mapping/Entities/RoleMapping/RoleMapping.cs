using AutoMapper;

namespace ProjectManagement.Core.Mapping.RoleMapping
{
    public partial class RoleMapping : Profile
    {
        #region Constructor
        public RoleMapping()
        {
            MapFromAddRoleCommandRequestModelToRoleEntity();
            MapFromUpdateRoleCommandRequestModelToRoleEntity();
            MappingFromRoleToRoleFullDataDto();
        }
        #endregion

    }
}
