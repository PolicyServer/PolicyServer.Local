// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using PolicyServer.Client;

namespace Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly PolicyServerClient _client;

        public HomeController(PolicyServerClient client)
        {
            _client = client;
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

        // if you are using the UsePolicyServerClaimsTransformation middleware, roles are mapped to claims
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
    }
}