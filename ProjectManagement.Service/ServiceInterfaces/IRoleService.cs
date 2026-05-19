using ProjectManagement.Domain.Entities;
using ProjectManagement.Service.Shared.PaginatedList;
using Microsoft.AspNetCore.Identity;

namespace ProjectManagement.Service.ServiceInterfaces
{
    public interface IRoleService
    {
        Task<List<Role>?> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task<IdentityResult> AddRoleAsync(Role role);
        Task<IdentityResult> EditAsync(Role entity);
        Task<IdentityResult> HardDeleteAsync(Role entity);
        Task<PaginatedList<Role>> GetPaginatedListAsync(int pageNumber = 1, int pageSize = 10);
        Task<bool> IsRoleNameExistAsync(string email);
        Task<bool> RoleIsUsedAsync(int roleId);
        Task<List<Role>> GetUserRolesAsync(int UserId);
    }
}
