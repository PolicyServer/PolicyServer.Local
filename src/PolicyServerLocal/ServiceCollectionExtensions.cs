using Microsoft.Extensions.Configuration;
using PolicyServer.Client;
using PolicyServerLocal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static PolicyServerBuilder AddPolicyServerClient(this IServiceCollection services, IConfiguration configuration)
        {
            var policy = new PolicyServerClient();
            configuration.Bind(policy);

            services.AddSingleton(policy);

            return new PolicyServerBuilder(services);
        }
    }
}