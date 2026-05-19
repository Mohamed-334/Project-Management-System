using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Context;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.BaseRepository;
using ProjectManagement.Infrastructure.Shared.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ProjectTask = ProjectManagement.Domain.Entities.ProjectTask;

namespace ProjectManagement.Infrastructure.Repository
{
    public class TaskRepository : BaseRepository<ProjectTask>, ITaskRepository
    {
        #region Fields
        private readonly DbSet<ProjectTask> _Tasks;
        private readonly IStringLocalizer<AppLocalization> _localizer;

        #endregion

        #region Methods
        public TaskRepository(AppDbContext context, IStringLocalizer<AppLocalization> localizer) : base(context)
        {
            _Tasks = context.Set<ProjectTask>();
            _localizer = localizer;
        }
        #endregion

        #region Actions

        #endregion
    }

}
