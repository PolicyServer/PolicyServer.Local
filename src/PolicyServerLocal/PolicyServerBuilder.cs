using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PolicyServer.Client
{
    public class PolicyServerBuilder
    {
        public IServiceCollection Services { get; }

        public PolicyServerBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public PolicyServerBuilder AddAuthorizationPermissionPolicies()
        {
            Services.AddAuthorization();
            Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            Services.AddTransient<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            Services.AddTransient<IAuthorizationHandler, PermissionHandler>();

            return this;
        }
    }
}