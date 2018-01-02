using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PolicyServerLocal;

namespace Microsoft.AspNetCore.Authorization
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly PolicyServerClient _client;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
            PolicyServerClient client,
            IHttpContextAccessor contextAccessor) : base(options)
        {
            _client = client;
            _contextAccessor = contextAccessor;
        }

        public static AuthorizationPolicy Allowed { get; set; } = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        public static AuthorizationPolicy Denied { get; set; } = new AuthorizationPolicyBuilder().RequireAssertion(c=>false).Build();

        public async override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // check static policies first
            var policy = await base.GetPolicyAsync(policyName);

            if (policy == null)
            {
                policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();
            }

            return policy;
        }
    }

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly PolicyServerClient _client;

        public PermissionHandler(PolicyServerClient client)
        {
            _client = client;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (await _client.HasPermissionAsync(context.User, requirement.Name))
            {
                context.Succeed(requirement);
            }
        }
    }

}
