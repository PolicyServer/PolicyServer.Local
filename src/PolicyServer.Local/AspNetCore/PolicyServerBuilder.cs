// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace PolicyServer.AspNetCore
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