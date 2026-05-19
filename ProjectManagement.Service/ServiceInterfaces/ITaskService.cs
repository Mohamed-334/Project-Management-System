using ProjectManagement.Service.Shared.Interface;
using ProjectManagement.Service.Shared.PaginatedList;

namespace TaskManagement.Service.ServiceInterfaces
{
    public interface ITaskService : IBaseService<ProjectManagement.Domain.Entities.ProjectTask>
    {
        Task<PaginatedList<ProjectManagement.Domain.Entities.ProjectTask>> GetProjectTasksPaginatedListAsync(int projectId, int pageNumber, int pageSize);
    }
}
