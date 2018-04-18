// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Host.AspNetCorePolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                // this sets up a default authorization policy for the application
                // in this case, authenticated users are required (besides controllers/actions that have [AllowAnonymous]
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            // this sets up authentication - for this demo we simply use a local cookie
            // typically authentication would be done using an external provider
            services.AddAuthentication("Cookies")
                .AddCookie("Cookies");

            // this sets up the PolicyServer client library and policy provider - configuration is loaded from appsettings.json
            services.AddPolicyServerClient(Configuration.GetSection("Policy"))
                .AddAuthorizationPermissionPolicies();

            // this adds the necessary handler for our custom medication requirement
            services.AddTransient<IAuthorizationHandler, MedicationRequirementHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();

            // add this middleware to make roles and permissions available as claims
            // this is mainly useful for using the classic [Authorize(Roles="foo")] and IsInRole functionality
            // this is not needed if you use the client library directly or the new policy-based authorization framework in ASP.NET Core
            app.UsePolicyServerClaims();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}