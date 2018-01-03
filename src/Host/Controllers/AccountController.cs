// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!string.IsNullOrWhiteSpace(userName))
            {
                var claims = new List<Claim>();

                if (userName == "alice")
                {
                    claims = new List<Claim>
                    {
                        new Claim("sub", "1"),
                        new Claim("name", "Alice"),
                    };
                }
                else if (userName == "bob")
                {
                    claims = new List<Claim>
                    {
                        new Claim("sub", "11"),
                        new Claim("name", "Bob"),
                    };
                }
                else
                {
                    claims = new List<Claim>
                    {
                        new Claim("sub", "21"),
                        new Claim("name", userName),
                        new Claim("sick", "very"),
                        new Claim("role", "customer")
                    };
                }

                var id = new ClaimsIdentity(claims, "password", "name", "role");
                var p = new ClaimsPrincipal(id);

                await HttpContext.SignInAsync(p);
                return LocalRedirect(returnUrl);
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult AccessDenied() => View();
    }
}