using AutoMapper;
using ProjectManagement.Core.Shared.BaseFeatures.Crud.Commands.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.Interface;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Authentication.Commands.Handlers
{
    public class CrudHandlerCommand<TEntity, TAddRequest, TUpdateRequest> : ResponseHandler
                                                where TEntity : BaseEntity
                                                where TAddRequest : IRequest<Response<string>>
                                                where TUpdateRequest : BaseModel, IRequest<Response<string>>,
                                                IRequestHandler<TAddRequest, Response<string>>,
                                                IRequestHandler<TUpdateRequest, Response<string>>,
                                                IRequestHandler<DeleteCommandRequestModelQuery, Response<string>>,
                                                IRequestHandler<SoftDeleteAndActivateCommandRequestQuery, Response<string>>

    {
        #region Fields
        protected readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        protected readonly IMapper _mapper;
        protected readonly IBaseService<TEntity> _baseService;
        protected readonly IAuthenticatedUserService _authenticatedUserService;
        #endregion

        #region Constructor
        public CrudHandlerCommand(IStringLocalizer<AppLocalization> stringLocalizer, IMapper mapper,
                                  IBaseService<TEntity> baseService,
                                  IAuthenticatedUserService authenticatedUserService)
            : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _baseService = baseService;
            _authenticatedUserService = authenticatedUserService;
        }
        #endregion

        #region Methods
        public async Task<Response<string>> Handle(TAddRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(request);
            var result = await _baseService.AddAsync(entity);
            if (result == _stringLocalizer[AppLocalizationKeys.AddFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.AddFailed]);
            return Success("");
        }

        public async Task<Response<string>> Handle(TUpdateRequest request, CancellationToken cancellationToken)
        {
            var entity = await _baseService.GetByIdAsync(request.Id);
            if (entity == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);

            var entityMapper = _mapper.Map(request, entity);
            var result = await _baseService.EditAsync(entityMapper);
            if (result == _stringLocalizer[AppLocalizationKeys.UpdateFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.UpdateFailed]);
            return Success<string>(_stringLocalizer[AppLocalizationKeys.Updated]);
        }
        public async Task<Response<string>> Handle(DeleteCommandRequestModelQuery request, CancellationToken cancellationToken)
        {
            var entity = await _baseService.GetByIdAsync(request.Id);
            if (entity == null)
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            var result = await _baseService.HardDeleteAsync(entity);
            if (result == _stringLocalizer[AppLocalizationKeys.UpdateFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.DeletedFailed]);
            return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
        }
        public async Task<Response<string>> Handle(SoftDeleteAndActivateCommandRequestQuery request, CancellationToken cancellationToken)
        {
            var entity = await _baseService.GetByIdAsync(request.Id);
            if (entity == null)
                return NotFound<string>(_stringLocalizer[AppLocalizationKeys.NotFound]);
            entity.IsDeleted = !(entity.IsDeleted);
            entity.DeletionDate = DateTime.UtcNow;
            entity.DeleterName = _authenticatedUserService.GetAuthenticatedUserName();
            var result = await _baseService.EditAsync(entity);

            if (result == _stringLocalizer[AppLocalizationKeys.DeletedFailed])
                return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.DeletedFailed]);
            if (entity.IsDeleted)
                return Deleted<string>(_stringLocalizer[AppLocalizationKeys.Deleted]);
            return Success<string>(_stringLocalizer[AppLocalizationKeys.Activated]);
        }

        #endregion
    }
}
