using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Shared.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(ICollection<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(ICollection<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task DeleteRangeAsync(ICollection<TEntity> entities);
        IQueryable<TEntity> GetTableAsTracking();
        IQueryable<TEntity> GetTableNoTracking();
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();
    }
}
