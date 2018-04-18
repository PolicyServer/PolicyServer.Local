// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PolicyServer.Runtime.Client.AspNetCore
{
    /// <summary>
    /// Middleware to automatically turn application roles and permissions into claims
    /// </summary>
    public class PolicyServerClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyServerClaimsMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public PolicyServerClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IPolicyServerRuntimeClient client)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var policy = await client.EvaluateAsync(context.User);

                var roleClaims = policy.Roles.Select(x => new Claim("role", x));
                var permissionClaims = policy.Permissions.Select(x => new Claim("permission", x));

                var id = new ClaimsIdentity("PolicyServerMiddleware", "name", "role");
                id.AddClaims(roleClaims);
                id.AddClaims(permissionClaims);

                context.User.AddIdentity(id);
            }

            await _next(context);
        }
    }
}