using ProjectManagement.Core.Shared.ValidationBase;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProjectManagement.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection Services)
        {
            // Register core dependencies here
            // Example: Services.AddScoped<IMyCoreService, MyCoreService>();

            // Register MediatR for CQRS pattern
            Services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // Register AutoMapper for object mapping]
            Services.AddAutoMapper(Assembly.GetExecutingAssembly());


            // Get Validators
            Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBase<,>));

            Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            return Services;
        }
    }
}
