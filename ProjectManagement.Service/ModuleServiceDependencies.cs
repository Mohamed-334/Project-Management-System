using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Service.Service;
using ProjectManagement.Service.ServiceInterfaces;
using TaskManagement.Service.ServiceInterfaces;

namespace ProjectManagement.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            // Register service dependencies here
            // Example: services.AddScoped<IMyService, MyService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            return services;
        }
    }
}
