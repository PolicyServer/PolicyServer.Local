// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PolicyServer.AspNetCore;
using PolicyServer.Client;
using PolicyServer.Local;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Helper class to configure DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the policy server client.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static PolicyServerBuilder AddPolicyServerClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Policy>(configuration);
            services.AddSingleton(r => r.GetRequiredService<IOptions<Policy>>().Value);

            services.AddTransient<IPolicyServerClient, PolicyServerClient>();

            return new PolicyServerBuilder(services);
        }
    }
}