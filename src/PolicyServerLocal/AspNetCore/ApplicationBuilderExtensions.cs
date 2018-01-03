// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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