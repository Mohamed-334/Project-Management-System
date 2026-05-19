using AutoMapper;
using ProjectManagement.Core.Features.ApplicationUser.DTO;
using ProjectManagement.Core.Features.ApplicationUser.Queries.RequestModels;
using ProjectManagement.Core.Features.Roles.Dto;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.ApplicationUser.Queries.Handler
{
    public class UserHandlerQuery : ResponseHandler,
                                    IRequestHandler<GetUsersListQueryRequestModel, Response<List<UserFullDataDto>>>,
                                    IRequestHandler<GetUserByIdQueryRequestModel, Response<UserFullDataDto>>,
                                    IRequestHandler<GetUsersPaginatedListQueryRequestModel, Response<PaginatedList<UserFullDataDto>>>,
                                    IRequestHandler<GetUserRolesQueryRequestModel, Response<List<RoleFullDataDto>>>
    {

        #region Fields
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public UserHandlerQuery(IUserService userService, IStringLocalizer<AppLocalization> stringLocalizer, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRoleService roleService) : base(stringLocalizer)
        {
            _userService = userService;
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
        }
        #endregion

        #region Methods
        public async Task<Response<List<UserFullDataDto>>> Handle(GetUsersListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var UserList = await _userService.GetAllAsync();
            if (UserList == null)
                return NotFound<List<UserFullDataDto>>(_stringLocalizer[AppLocalizationKeys.UserIsNotFound]);
            var UserFullDataDtoList = _mapper.Map<List<UserFullDataDto>>(UserList);
            return Success(UserFullDataDtoList, _stringLocalizer[AppLocalizationKeys.Success], new { TotalCount = UserFullDataDtoList.Count });
        }

        public async Task<Response<UserFullDataDto>> Handle(GetUserByIdQueryRequestModel request, CancellationToken cancellationToken)
        {
            var User = await _userService.GetByIdAsync(request.UserId);
            if (User == null)
                return NotFound<UserFullDataDto>(_stringLocalizer[AppLocalizationKeys.UserIsNotFound]);

            var UserFullDataDto = _mapper.Map<UserFullDataDto>(User);
            return Success(UserFullDataDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<PaginatedList<UserFullDataDto>>> Handle(GetUsersPaginatedListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var PaginatedList = await _userService.GetPaginatedListAsync(request.PageNumber, request.PageSize);
            if (PaginatedList == null || PaginatedList.Data.Count == 0)
                return NotFound<PaginatedList<UserFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var UserFullDataDtoList = _mapper.Map<List<UserFullDataDto>>(PaginatedList.Data);
            var paginatedListDto = PaginatedList<UserFullDataDto>.Success(UserFullDataDtoList, PaginatedList.TotalCount, PaginatedList.CurrentPage, PaginatedList.PageSize);
            return Success(paginatedListDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<List<RoleFullDataDto>>> Handle(GetUserRolesQueryRequestModel request, CancellationToken cancellationToken)
        {
            var User = await _userService.GetByIdAsync(request.UserId);
            if (User == null)
                return NotFound<List<RoleFullDataDto>>(_stringLocalizer[AppLocalizationKeys.UserIsNotFound]);
            var roles = await _roleService.GetUserRolesAsync(User.Id);
            if (roles == null || roles.Count <= 0)
                return NotFound<List<RoleFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var RoleFullDataDtoList = _mapper.Map<List<RoleFullDataDto>>(roles);
            return Success(RoleFullDataDtoList, _stringLocalizer[AppLocalizationKeys.Success], new { TotalCount = RoleFullDataDtoList.Count });
        }
        #endregion


    }
}
