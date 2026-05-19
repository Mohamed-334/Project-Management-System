using AutoMapper;

namespace ProjectManagement.Core.Mapping.UserMapping
{
    public partial class UserMapping : Profile
    {
        #region Constructor
        public UserMapping()
        {
            MappingFromUserToUserFullDataDto();
            MappingFromUpdateUserCommandRequestModelToUser();
        }
        #endregion
    }
}
