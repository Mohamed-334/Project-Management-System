using ProjectManagement.Core.Features.Authentication.Commands.RequestModels;
using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Core.Mapping.AuthenticationMapping
{
    public partial class AuthenticationMapping
    {
        #region Methods
        public void MapFromSignupCommandRequestModelToUser()
        {
            CreateMap<SignUpCommandRequestModel, User>()
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .AfterMap<MetaMappingDataBasedOnDestination<SignUpCommandRequestModel, User>>();
        }
        #endregion
    }
}
