using AutoMapper;
using ProjectManagement.Core.Shared.BaseFeatures.Crud.Queries.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.Interface;
using ProjectManagement.Service.Shared.PaginatedList;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Shared.BaseFeatures.Crud.Queries.Handlers
{
    public class CrudHandlerQuery<TEntity, TDto> : ResponseHandler
                                        where TEntity : BaseEntity
                                        where TDto : IRequest<Response<string>>,
                                        IRequestHandler<TDto, Response<string>>,
                                        IRequestHandler<GetByIdRequestModelQuery<TDto>, Response<TDto>>,
                                        IRequestHandler<GetListRequestModelQuery<TDto>, Response<List<TDto>>>,
                                        IRequestHandler<GetPaginatedListRequestModelQuery<TDto>, Response<PaginatedList<TDto>>>

    {
        #region Fields
        protected readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        protected readonly IMapper _mapper;
        protected readonly IAuthenticationService _authenticationService;
        protected readonly IBaseService<TEntity> _baseService;
        protected readonly IAuthenticatedUserService _authenticatedUserService;
        #endregion

        #region Constructor
        public CrudHandlerQuery(IStringLocalizer<AppLocalization> stringLocalizer, IMapper mapper,
                                  IAuthenticationService authenticationService, IBaseService<TEntity> baseService,
                                  IAuthenticatedUserService authenticatedUserService)
            : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _baseService = baseService;
            _authenticatedUserService = authenticatedUserService;
        }
        #endregion

        #region Methods
        public async Task<Response<TDto>> Handle(GetByIdRequestModelQuery<TDto> request, CancellationToken cancellationToken)
        {
            var entity = await _baseService.GetByIdAsync(request.Id);
            if (entity == null)
                return NotFound<TDto>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var entityDto = _mapper.Map<TDto>(entity);
            return Success(entityDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        public async Task<Response<List<TDto>>> Handle(GetListRequestModelQuery<TDto> request, CancellationToken cancellationToken)
        {
            var entities = await _baseService.GetAllAsync();
            if (entities == null)
                return NotFound<List<TDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var entitiesDto = _mapper.Map<List<TDto>>(entities);
            return Success(entitiesDto, _stringLocalizer[AppLocalizationKeys.Success], new { TotalCount = entitiesDto.Count });

        }

        public async Task<Response<PaginatedList<TDto>>> Handle(GetPaginatedListRequestModelQuery<TDto> request, CancellationToken cancellationToken)
        {
            var PaginatedList = await _baseService.GetPaginatedListAsync(request.PageNumber, request.PageSize);
            if (PaginatedList == null || PaginatedList.Data.Count == 0)
                return NotFound<PaginatedList<TDto>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var EntityFullDataDtoList = _mapper.Map<List<TDto>>(PaginatedList.Data);
            var paginatedListDto = PaginatedList<TDto>.Success(EntityFullDataDtoList, PaginatedList.TotalCount, PaginatedList.CurrentPage, PaginatedList.PageSize);
            return Success(paginatedListDto, _stringLocalizer[AppLocalizationKeys.Success]);
        }
        public async Task<Response<List<DropDown>>> Handle(GetDropDownRequestModelQuery request, CancellationToken cancellationToken)
        {
            var entities = await _baseService.GetAllAsync();
            if (entities == null)
                return NotFound<List<DropDown>>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var dto = _mapper.Map<List<DropDown>>(entities);
            return Success(dto, _stringLocalizer[AppLocalizationKeys.Success]);
        }

        #endregion
    }
}
