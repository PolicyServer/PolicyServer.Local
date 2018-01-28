// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PolicyServer.Client;

namespace Host.AspNetCorePolicy
{
    public class MedicationRequirementHandler : AuthorizationHandler<MedicationRequirement>
    {
        private readonly IPolicyServerClient _client;

        public MedicationRequirementHandler(IPolicyServerClient client)
        {
            _client = client;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MedicationRequirement requirement)
        {
            var user = context.User; var allowed = false;

            if (await _client.HasPermissionAsync(user, "PrescribeMedication"))
            {
                if (requirement.Amount <= 10) allowed = true;
                else allowed = await _client.IsInRoleAsync(user, "doctor");

                if (allowed || requirement.MedicationName == "placebo")
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}