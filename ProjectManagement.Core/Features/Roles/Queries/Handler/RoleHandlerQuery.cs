using AutoMapper;
using ProjectManagement.Core.Features.Roles.Dto;
using ProjectManagement.Core.Features.Roles.Queries.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Roles.Queries.Handler
{
    public class RoleHandlerQuery : ResponseHandler,
                                    IRequestHandler<GetRolesListQueryRequestModel, Response<List<RoleFullDataDto>>>,
                                    IRequestHandler<GetRoleByIdQueryRequestModel, Response<RoleFullDataDto>>,
                                    IRequestHandler<GetRolesPaginatedListQueryRequestModel, Response<PaginatedList<RoleFullDataDto>>>
    {

        #region Fields
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public RoleHandlerQuery(IRoleService roleService, IStringLocalizer<AppLocalization> stringLocalizer, IMapper mapper) : base(stringLocalizer)
        {
            _roleService = roleService;
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<Response<List<RoleFullDataDto>>> Handle(GetRolesListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var RoleList = await _roleService.GetAllAsync();
            if (RoleList == null)
                return NotFound<List<RoleFullDataDto>>(_stringLocalizer[AppLocalizationKeys.UserIsNotFound]);
            var RoleFullDataDtoList = _mapper.Map<List<RoleFullDataDto>>(RoleList);
            return Success(RoleFullDataDtoList, _stringLocalizer[AppLocalizationKeys.Success], new { TotalCount = RoleFullDataDtoList.Count });
        }

        public async Task<Response<RoleFullDataDto>> Handle(GetRoleByIdQueryRequestModel request, CancellationToken cancellationToken)
        {
            var Role = await _roleService.GetByIdAsync(request.RoleId);
            if (Role == null)
                return NotFound<RoleFullDataDto>(_stringLocalizer[AppLocalizationKeys.UserIsNotFound]);
            var RoleFullDataDto = _mapper.Map<RoleFullDataDto>(Role);
            return Success(RoleFullDataDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<PaginatedList<RoleFullDataDto>>> Handle(GetRolesPaginatedListQueryRequestModel request, CancellationToken cancellationToken)
        {
            var PaginatedList = await _roleService.GetPaginatedListAsync(request.PageNumber, request.PageSize);
            if (PaginatedList == null || PaginatedList.Data.Count == 0)
                return NotFound<PaginatedList<RoleFullDataDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var RoleFullDataDtoList = _mapper.Map<List<RoleFullDataDto>>(PaginatedList.Data);
            var paginatedListDto = PaginatedList<RoleFullDataDto>.Success(RoleFullDataDtoList, PaginatedList.TotalCount, PaginatedList.CurrentPage, PaginatedList.PageSize);
            return Success(paginatedListDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }
        #endregion


    }
}
