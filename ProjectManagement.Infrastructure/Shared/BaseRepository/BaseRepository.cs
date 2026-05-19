using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using ProjectManagement.Infrastructure.Context;
using ProjectManagement.Infrastructure.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProjectManagement.Infrastructure.Shared.BaseRepository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        #region Props

        protected readonly AppDbContext _context;

        #endregion

        #region Constructor(s)
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Actions
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {

            return await _context.Set<TEntity>().FindAsync(id);
        }


        public IQueryable<TEntity> GetTableNoTracking()
        {
            return _context.Set<TEntity>()
                           .AsNoTracking()
                           .Where(x => !x.IsDeleted)
                           .AsQueryable();
        }


        public virtual async Task AddRangeAsync(ICollection<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();

        }
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();

        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public virtual async Task DeleteRangeAsync(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



        public IDbContextTransaction BeginTransaction()
        {


            return _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();

        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();

        }

        public IQueryable<TEntity> GetTableAsTracking()
        {
            return _context.Set<TEntity>()
                           .Where(x => !x.IsDeleted)
                           .AsQueryable();

        }

        public virtual async Task UpdateRangeAsync(ICollection<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
