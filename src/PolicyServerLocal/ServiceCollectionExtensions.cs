using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PolicyServerLocal
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPolicyServerClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Policy>(configuration);

            return services;
        }
    }
}