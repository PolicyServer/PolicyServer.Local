using PolicyServer.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePolicyServerClaimsTransformation(this IApplicationBuilder app)
        {
            app.UseMiddleware<PolicyServerMiddleware>();
            return app;
        }
    }
}