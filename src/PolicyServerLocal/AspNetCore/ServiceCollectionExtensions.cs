using Microsoft.Extensions.Configuration;
using PolicyServer.AspNetCore;
using PolicyServer.Client;
using PolicyServer.Local;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static PolicyServerBuilder AddPolicyServerClient(this IServiceCollection services, IConfiguration configuration)
        {
            var policy = new Policy();
            configuration.Bind(policy);

            services.AddSingleton(policy);
            services.AddTransient<PolicyServerClient>();

            return new PolicyServerBuilder(services);
        }
    }
}