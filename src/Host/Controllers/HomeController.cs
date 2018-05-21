// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Host.AspNetCorePolicy;
using PolicyServer.Runtime.Client;

namespace Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPolicyServerRuntimeClient _client;
        private readonly IAuthorizationService _authz;

        public HomeController(IPolicyServerRuntimeClient client, IAuthorizationService authz)
        {
            _client = client;
            _authz = authz;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Secure()
        {
            var result = await _client.EvaluateAsync(User);
            return View(result);
        }

        // if you are using the UsePolicyServerClaims middleware, roles are mapped to claims
        // this allows using the classic authorize attribute
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> NursesOnly()
        {
            // can also use the client library imperatively
            var isNurse = await _client.IsInRoleAsync(User, "nurse");

            return View("success");
        }

        // the preferred approach is to use the authorization policy system in ASP.NET Core
        // if you add the AuthorizationPermissionPolicies service, policy names are automatically mapped to permissions
        [Authorize("PerformSurgery")]
        public async Task<IActionResult> PerformSurgery()
        {
            // or imperatively
            var canPerformSurgery = await _client.HasPermissionAsync(User, "PerformSurgery");

            return View("success");
        }

        public async Task<IActionResult> PrescribeMedication(string name, int amount)
        {
            var requirement = new MedicationRequirement
            {
                Amount = amount,
                MedicationName = name
            };
            var result = await _authz.AuthorizeAsync(User, null, requirement);
            if (!result.Succeeded) return Forbid();

            return View("success");
        }
    }
}