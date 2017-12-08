using Microsoft.Extensions.Configuration;
using PolicyServerLocal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPolicyServerClient(this IServiceCollection services, IConfiguration configuration)
        {
            var policy = new Policy();
            configuration.Bind(policy);

            services.AddSingleton(policy);

            return services;
        }
    }
}