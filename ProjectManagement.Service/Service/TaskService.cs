using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.BaseService;
using ProjectManagement.Service.Shared.ExtensionMethods;
using ProjectManagement.Service.Shared.PaginatedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using static ProjectManagement.Domain.Enums.EnumExtensions;
using TaskManagement.Service.ServiceInterfaces;
using ProjectTask = ProjectManagement.Domain.Entities.ProjectTask;

namespace ProjectManagement.Service.Service
{
    public class TaskService : BaseService<ProjectTask>, ITaskService
    {
        #region Fields
        private readonly ITaskRepository _TaskRepository;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        #endregion

        #region Constructor
        public TaskService(IStringLocalizer<AppLocalization> stringLocalizer,
                         ITaskRepository TaskRepository,
                         IUserService userService,
                         IEmailService emailService) : base(TaskRepository, stringLocalizer)
        {
            _TaskRepository = TaskRepository;
            _userService = userService;
            _emailService = emailService;
        }
        #endregion

        #region Methods
        public async System.Threading.Tasks.Task<PaginatedList<ProjectTask>> GetProjectTasksPaginatedListAsync(int projectId, int pageNumber, int pageSize)
        {
            return await _baseRepository.GetTableNoTracking()
                .Where(t => t.ProjectId == projectId)
                .ToPaginatedListAsync(pageNumber, pageSize);
        }
        #endregion
    }
}
