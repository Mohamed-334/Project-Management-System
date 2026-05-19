using ProjectManagement.Core.Mapping.Shared;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;

namespace ProjectManagement.Core.Mapping.NotificationMapping
{
    public partial class NotificationMapping
    {
        #region Methods
        public void MappingFromNotificationToDropDown()
        {
            CreateMap<Notification, DropDown>()
                .AfterMap<MetaMappingDataBasedOnSource<Notification, DropDown>>();
        }
        #endregion
    }

}
