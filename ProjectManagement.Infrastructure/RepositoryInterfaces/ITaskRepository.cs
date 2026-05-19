using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Shared.Interfaces;
using ProjectTask = ProjectManagement.Domain.Entities.ProjectTask;

namespace ProjectManagement.Infrastructure.RepositoryInterfaces
{
    public interface ITaskRepository : IBaseRepository<ProjectTask>
    {
    }
}
