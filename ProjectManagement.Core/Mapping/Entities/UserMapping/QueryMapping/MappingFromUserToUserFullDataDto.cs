using ProjectManagement.Core.Features.ApplicationUser.DTO;
using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Core.Mapping.UserMapping
{
    public partial class UserMapping
    {
        public void MappingFromUserToUserFullDataDto()
        {
            CreateMap<User, UserFullDataDto>()
                .ForMember(dest => dest.Role, opt => opt
                        .MapFrom(src => src.UserRoles!.FirstOrDefault()!.Role!.Name))
                .ForMember(dest => dest.RoleId, opt => opt
                        .MapFrom(src => src.UserRoles!.FirstOrDefault()!.RoleId))
                .AfterMap<MetaMappingDataBasedOnSource<User, UserFullDataDto>>();
        }
    }

}
