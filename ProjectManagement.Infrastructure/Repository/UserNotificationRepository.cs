using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Context;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.BaseRepository;
using ProjectManagement.Infrastructure.Shared.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Infrastructure.Repository
{
    public class UserNotificationRepository : BaseRepository<UserNotification>, IUserNotificationRepository
    {
        #region Fields
        private readonly DbSet<UserNotification> _otp;
        private readonly IStringLocalizer<AppLocalization> _localizer;

        #endregion

        #region Methods
        public UserNotificationRepository(AppDbContext context, IStringLocalizer<AppLocalization> localizer) : base(context)
        {
            _otp = context.Set<UserNotification>();
            _localizer = localizer;
        }
        #endregion

        #region Actions

        #endregion
    }
}
