using ProjectManagement.Service.Shared.PaginatedList;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProjectManagement.Service.Shared.Interface
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<string> AddAsync(TEntity entity);
        Task<string> EditAsync(TEntity entity);
        Task<string> HardDeleteAsync(TEntity entity);
        Task<string> SoftDeleteAndActivationAsync(int id);
        Task<PaginatedList<TEntity>> GetPaginatedListAsync(int pageNumber = 1, int pageSize = 10);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
