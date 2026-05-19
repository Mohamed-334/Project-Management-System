using ProjectManagement.Core.Features.ApplicationUser.Commands.RequestModels;
using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Core.Mapping.UserMapping
{
    public partial class UserMapping
    {
        public void MappingFromUpdateUserCommandRequestModelToUser()
        {
            CreateMap<UpdateUserCommandRequestQuery, User>()
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .AfterMap<MetaMappingDataBasedOnDestination<UpdateUserCommandRequestQuery, User>>();

        }
    }
}
