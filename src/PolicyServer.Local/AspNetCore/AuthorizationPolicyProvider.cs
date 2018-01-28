// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PolicyServer.Client;

namespace Microsoft.AspNetCore.Authorization
{
    /// <summary>
    /// Authorization policy provider to automatically turn all permissions of a user into a ASP.NET Core authorization policy
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider" />
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly IPolicyServerClient _client;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationPolicyProvider"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="client">The client.</param>
        /// <param name="contextAccessor">The context accessor.</param>
        public AuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
            IPolicyServerClient client,
            IHttpContextAccessor contextAccessor) : base(options)
        {
            _client = client;
            _contextAccessor = contextAccessor;
        }


        /// <summary>
        /// Gets a <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> from the given <paramref name="policyName" />
        /// </summary>
        /// <param name="policyName">The policy name to retrieve.</param>
        /// <returns>
        /// The named <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" />.
        /// </returns>
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

    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }

    internal class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPolicyServerClient _client;

        public PermissionHandler(IPolicyServerClient client)
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