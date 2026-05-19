using ProjectManagement.Service.Service;
using ProjectManagement.Service.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;

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
            return services;
        }
    }
}
