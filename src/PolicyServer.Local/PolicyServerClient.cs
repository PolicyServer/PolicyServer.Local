// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PolicyServer.Local;

namespace PolicyServer.Client
{
    public class PolicyServerClient
    {
        private readonly Policy _policy;

        public PolicyServerClient(Policy policy)
        {
            _policy = policy;
        }

        public async Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role)
        {
            var policy = await EvaluateAsync(user);
            return policy.Roles.Contains(role);
        }

        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission)
        {
            var policy = await EvaluateAsync(user);
            return policy.Permissions.Contains(permission);
        }

        public Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return _policy.EvaluateAsync(user);
        }
    }
}