// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using PolicyServer.AspNetCore;
using PolicyServer.Client;
using PolicyServer.Local;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static PolicyServerBuilder AddPolicyServerClient(this IServiceCollection services, IConfiguration configuration)
        {
            var policy = new Policy();
            configuration.Bind(policy);

            services.AddSingleton(policy);
            services.AddTransient<PolicyServerClient>();

            return new PolicyServerBuilder(services);
        }
    }
}