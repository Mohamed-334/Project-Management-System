using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using ProjectManagement.Service.Shared.BaseService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using static ProjectManagement.Domain.Enums.EnumExtensions;
using Project = ProjectManagement.Domain.Entities.Project;

namespace ProjectManagement.Service.Service
{
    public class ProjectService : BaseService<Project>, IProjectService
    {
        #region Fields
        private readonly IProjectRepository _ProjectRepository;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        #endregion

        #region Constructor
        public ProjectService(IStringLocalizer<AppLocalization> stringLocalizer,
                         IProjectRepository ProjectRepository,
                         IUserService userService,
                         IEmailService emailService) : base(ProjectRepository, stringLocalizer)
        {
            _ProjectRepository = ProjectRepository;
            _userService = userService;
            _emailService = emailService;
        }
        #endregion

        #region Methods


        #endregion
    }
}
