using ProjectManagement.Infrastructure.Context.Interceptors;
using ProjectManagement.Infrastructure.Repository;
using ProjectManagement.Infrastructure.RepositoryInterfaces;
using ProjectManagement.Infrastructure.Shared.BaseRepository;
using ProjectManagement.Infrastructure.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagement.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection Services)
        {
            // Register service dependencies here
            // Example: services.AddScoped<IMyService, MyService>();
            Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            Services.AddScoped(typeof(IOtpRepository), typeof(OtpRepository));
            Services.AddScoped(typeof(INotificationRepository), typeof(NotificationRepository));
            Services.AddScoped(typeof(IUserNotificationRepository), typeof(UserNotificationRepository));
            Services.AddScoped(typeof(ITaskRepository), typeof(TaskRepository));
            Services.AddScoped(typeof(IProjectRepository), typeof(ProjectRepository));
            Services.AddScoped<LoggerSaveChangesInterceptor>();
            return Services;
        }
    }
}
