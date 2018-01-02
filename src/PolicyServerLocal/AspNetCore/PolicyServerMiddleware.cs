using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PolicyServer.Client;

namespace PolicyServer.AspNetCore
{
    public class PolicyServerMiddleware
    {
        private readonly RequestDelegate _next;

        public PolicyServerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext ctx, PolicyServerClient client)
        {
            if (ctx.User.Identity.IsAuthenticated)
            {
                var policy = await client.EvaluateAsync(ctx.User);
                var roleClaims = policy.Roles.Select(x => new Claim("role", x));
                var permissionClaims = policy.Permissions.Select(x => new Claim("permission", x));

                // todo: make scheme configurable
                var id = new ClaimsIdentity("PolicyServerMiddleware", "name", "role");
                id.AddClaims(roleClaims);
                id.AddClaims(permissionClaims);
                ctx.User.AddIdentity(id);
            }

            await _next(ctx);
        }
    }
}
