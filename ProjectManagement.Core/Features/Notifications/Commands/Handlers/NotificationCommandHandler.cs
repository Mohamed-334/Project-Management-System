using AutoMapper;
using ProjectManagement.Core.Features.Notifications.Commands.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Notifications.Commands.Handler
{
    public class NotificationCommandHandler : ResponseHandler, IRequestHandler<AddNotificationCommandRequestModel, Response<string>>,
                                                         IRequestHandler<SendNotificationCommandRequestModel, Response<string>>,
                                                         IRequestHandler<UpdateNotificationCommandRequestModel, Response<string>>,
                                                         IRequestHandler<DeleteNotificationCommandRequestModel, Response<string>>,
                                                         IRequestHandler<SoftDeleteAndActivateNotificationCommandRequestModel, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        #endregion

        #region Constructor
        public NotificationCommandHandler(IStringLocalizer<AppLocalization> stringLocalizer,
                                  IMapper mapper,
                                  INotificationService notificationService,
                                  IAuthenticatedUserService authenticatedUserService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _notificationService = notificationService;
            _authenticatedUserService = authenticatedUserService;
        }
        #endregion

        #region Methods
        public async Task<Response<string>> Handle(AddNotificationCommandRequestModel request, CancellationToken cancellationToken)
        {
            var Notification = _mapper.Map<Notification>(request);
            var result = await _notificationService.AddAsync(Notification);
            if (result == _stringLocalizer[AppLocalizationKeys.AddFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.AddFailed]);
            return Success("", Meta: new { Id = Notification.Id });
        }

        public async Task<Response<string>> Handle(UpdateNotificationCommandRequestModel request, CancellationToken cancellationToken)
        {
            var Notification = await _notificationService.GetByIdAsync(request.Id);
            if (Notification == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);

            var NotificationMapper = _mapper.Map(request, Notification);
            var result = await _notificationService.EditAsync(NotificationMapper);
            if (result == _stringLocalizer[AppLocalizationKeys.UpdateFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.UpdateFailed]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Updated]);
        }

        public async Task<Response<string>> Handle(DeleteNotificationCommandRequestModel request, CancellationToken cancellationToken)
        {
            var Notification = await _notificationService.GetByIdAsync(request.Id);
            if (Notification == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var result = await _notificationService.HardDeleteAsync(Notification);
            if (result == _stringLocalizer[AppLocalizationKeys.DeletedFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.DeletedFailed]);
            return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
        }

        public async Task<Response<string>> Handle(SoftDeleteAndActivateNotificationCommandRequestModel request, CancellationToken cancellationToken)
        {
            var Notification = await _notificationService.GetByIdAsync(request.Id);
            if (Notification == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            Notification.IsDeleted = !(Notification.IsDeleted);
            Notification.DeletionDate = DateTime.UtcNow;
            Notification.DeleterName = _authenticatedUserService.GetAuthenticatedUserName();
            var result = await _notificationService.EditAsync(Notification);

            if (result == _stringLocalizer[AppLocalizationKeys.DeletedFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.DeletedFailed]);
            if (Notification.IsDeleted)
                return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
            return Success<string>(msg: _stringLocalizer[AppLocalizationKeys.Activated]);
        }

        public async Task<Response<string>> Handle(SendNotificationCommandRequestModel request, CancellationToken cancellationToken)
        {
            var Notification = _mapper.Map<Notification>(request);

            var result = await _notificationService.SendNotification(Notification, request.Users);
            if (result == _stringLocalizer[AppLocalizationKeys.FailedToSendNotification])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.FailedToSendNotification]);

            return Success("", Meta: new { Id = Notification.Id });
        }
        #endregion

    }
}


