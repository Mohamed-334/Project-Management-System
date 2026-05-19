using AutoMapper;
using ProjectManagement.Core.Features.Notifications.Dto;
using ProjectManagement.Core.Features.Notifications.Queries.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Notifications.Queries.Handler
{
    public class NotificationHandlerQuery : ResponseHandler,
                                    IRequestHandler<GetNotificationsListQueryRequestModel, Response<List<NotificationFullDataDto>>>,
                                    IRequestHandler<GetNotificationByIdQueryRequestModel, Response<NotificationFullDataDto>>,
                                    IRequestHandler<GetNotificationsPaginatedListQueryRequestModel, Response<PaginatedList<NotificationFullDataDto>>>,
                                    IRequestHandler<GetNotificationsDropDownQueryRequestModel, Response<List<DropDown>>>
    {

        #region Fields
        private readonly INotificationService _notificationService;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _notificationMapping;
        #endregion

        #region Constructor
        public NotificationHandlerQuery(INotificationService notificationService, IStringLocalizer<AppLocalization> stringLocalizer, IHttpContextAccessor httpContextAccessor, IMapper notificationMapping) : base(stringLocalizer)
        {
            _notificationService = notificationService;
            _stringLocalizer = stringLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _notificationMapping = notificationMapping;
        }
        #endregion

        #region Methods
        public async Task<Response<List<NotificationFullDataDto>>> Handle(GetNotificationsListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var Notifications = await _notificationService.GetAllAsync();
            if (Notifications == null)
                return NotFound<List<NotificationFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var NotificationsDto = _notificationMapping.Map<List<NotificationFullDataDto>>(Notifications).ToList();
            return Success(NotificationsDto, _stringLocalizer[AppLocalizationKeys.Success], new { TotalCount = NotificationsDto.Count });
        }

        public async Task<Response<NotificationFullDataDto>> Handle(GetNotificationByIdQueryRequestModel request, CancellationToken cancellationToken)
        {
            var Notification = await _notificationService.GetByIdAsync(request.Id);
            if (Notification == null)
                return NotFound<NotificationFullDataDto>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var NotificationDto = _notificationMapping.Map<NotificationFullDataDto>(Notification);
            return Success(NotificationDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<PaginatedList<NotificationFullDataDto>>> Handle(GetNotificationsPaginatedListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var PaginatedList = await _notificationService.GetPaginatedListAsync(request.PageNumber, request.PageSize);
            if (PaginatedList == null)
                return NotFound<PaginatedList<NotificationFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var NotificationFullDataDtoList = _notificationMapping.Map<List<NotificationFullDataDto>>(PaginatedList.Data).ToList();
            var paginatedListDto = PaginatedList<NotificationFullDataDto>.Success(NotificationFullDataDtoList, PaginatedList.TotalCount, PaginatedList.CurrentPage, PaginatedList.PageSize);
            return Success(paginatedListDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }
        public async Task<Response<List<DropDown>>> Handle(GetNotificationsDropDownQueryRequestModel request, CancellationToken cancellationToken)
        {
            var Notifications = await _notificationService.GetAllAsync();
            if (Notifications == null)
                return NotFound<List<DropDown>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var DropDowns = _notificationMapping.Map<List<DropDown>>(Notifications).ToList();
            return Success(DropDowns, _stringLocalizer[AppLocalizationKeys.Success]);
        }
        #endregion

    }
}
