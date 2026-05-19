using ProjectManagement.Core.Features.Notifications.Commands.RequestModels;
using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Core.Mapping.NotificationMapping
{
    public partial class NotificationMapping
    {
        #region Methods
        public void MapFromAddNotificationCommandRequestModelToNotificationEntity()
        {
            CreateMap<AddNotificationCommandRequestModel, Notification>()
            .AfterMap<MetaMappingDataBasedOnDestination<AddNotificationCommandRequestModel, Notification>>();
            #endregion
        }
    }
}
