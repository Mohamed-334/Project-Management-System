using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using ProjectManagement.Infrastructure.Shared.Interfaces;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.Shared.ExtensionMethods;
using ProjectManagement.Service.Shared.Interface;
using ProjectManagement.Service.Shared.PaginatedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Service.Shared.BaseService
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        #region Vars / Props

        protected readonly IBaseRepository<TEntity> _baseRepository;
        protected readonly IStringLocalizer<AppLocalization> _stringLocalizer;

        #endregion

        #region Constructor(s)
        public BaseService(IBaseRepository<TEntity> baseRepository, IStringLocalizer<AppLocalization> stringLocalizer)
        {
            _baseRepository = baseRepository;
            _stringLocalizer = stringLocalizer;
        }

        #endregion

        #region Actions
        public async virtual Task<List<TEntity>> GetAllAsync()
        {
            var List = await _baseRepository
                            .GetTableNoTracking()
                            .ToListAsync();
            return List;
        }
        public async virtual Task<TEntity> GetByIdAsync(int id)
        {
            var Entity = await _baseRepository
                            .GetByIdAsync(id);
            return Entity;
        }
        public async Task<string> AddAsync(TEntity entity)
        {
            try
            {
                await _baseRepository.AddAsync(entity);
                return _stringLocalizer[AppLocalizationKeys.Success];
            }
            catch (Exception ex)
            {
                return _stringLocalizer[AppLocalizationKeys.AddFailed];
            }
        }
        public async Task<string> EditAsync(TEntity entity)
        {
            try
            {
                await _baseRepository.UpdateAsync(entity);
                return _stringLocalizer[AppLocalizationKeys.Success];
            }
            catch (Exception ex)
            {
                return _stringLocalizer[AppLocalizationKeys.UpdateFailed];
            }
        }
        public async Task<string> HardDeleteAsync(TEntity entity)
        {
            try
            {
                await _baseRepository.DeleteAsync(entity);
                return _stringLocalizer[AppLocalizationKeys.Success];
            }
            catch
            {
                return _stringLocalizer[AppLocalizationKeys.DeletedFailed];
            }
        }
        public async Task<string> SoftDeleteAndActivationAsync(int id)
        {
            var entity = await _baseRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return _stringLocalizer[AppLocalizationKeys.NotFound];
            }
            entity.IsDeleted = !entity.IsDeleted;
            try
            {
                await EditAsync(entity);
                return _stringLocalizer[AppLocalizationKeys.Success];
            }
            catch (Exception ex)
            {
                return _stringLocalizer[AppLocalizationKeys.DeletedFailed];
            }
        }
        public async virtual Task<PaginatedList<TEntity>> GetPaginatedListAsync(int pageNumber = 1, int pageSize = 10)
        {
            var Queryable = _baseRepository
                            .GetTableNoTracking()
                            .AsQueryable();

            var PaginatedList = await Queryable
                .ToPaginatedListAsync(pageNumber, pageSize);
            return PaginatedList;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _baseRepository.BeginTransaction();
        }
        #endregion
    }
}
