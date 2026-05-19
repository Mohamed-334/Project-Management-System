using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Context;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.BaseRepository;
using ProjectManagement.Infrastructure.Shared.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Infrastructure.Repository
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        #region Fields
        private readonly DbSet<Project> _Projects;
        private readonly IStringLocalizer<AppLocalization> _localizer;

        #endregion

        #region Constructor
        public ProjectRepository(AppDbContext context, IStringLocalizer<AppLocalization> localizer) : base(context)
        {
            _Projects = context.Set<Project>();
            _localizer = localizer;
        }
        #endregion

        #region Methods

        #endregion
    }

}
