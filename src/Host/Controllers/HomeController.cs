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
        private readonly PolicyServerClient _policy;

        public HomeController(PolicyServerClient policy)
        {
            _policy = policy;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Secure()
        {
            var result = await _policy.EvaluateAsync(User);
            return View(result);
        }

        //[Authorize(Roles="doctor,nurse")]
        //[Authorize("SeePatients")]

    }
}