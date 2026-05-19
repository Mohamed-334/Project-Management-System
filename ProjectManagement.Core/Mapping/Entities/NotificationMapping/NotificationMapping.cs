using AutoMapper;

namespace ProjectManagement.Core.Mapping.NotificationMapping
{
    public partial class NotificationMapping : Profile
    {
        #region Constructor
        public NotificationMapping()
        {
            MapFromAddNotificationCommandRequestModelToNotificationEntity();
            MapFromSendNotificationCommandRequestModelToNotificationEntity();
            MapFromUpdateNotificationCommandRequestModelToNotificationEntity();
            MappingFromNotificationToNotificationFullDataDto();
            MappingFromNotificationToDropDown();
        }
        #endregion

    }
}
