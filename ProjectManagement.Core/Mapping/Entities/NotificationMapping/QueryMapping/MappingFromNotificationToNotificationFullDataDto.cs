using ProjectManagement.Core.Features.Notifications.Dto;
using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Core.Mapping.NotificationMapping
{
    public partial class NotificationMapping
    {
        #region Methods
        public void MappingFromNotificationToNotificationFullDataDto()
        {
            CreateMap<Notification, NotificationFullDataDto>()
                .AfterMap<MetaMappingDataBasedOnSource<Notification, NotificationFullDataDto>>();
        }
        #endregion
    }

}
