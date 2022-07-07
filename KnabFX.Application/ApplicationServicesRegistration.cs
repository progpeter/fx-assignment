using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace KnabFX.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
