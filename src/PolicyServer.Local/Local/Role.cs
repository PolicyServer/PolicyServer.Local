// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PolicyServer.Local
{
    public class Role
    {
        public string Name { get; set; }

        public List<string> Subjects { get; set; } = new List<string>();
        public List<string> IdentityRoles { get; set; } = new List<string>();

        internal bool Evaluate(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var sub = user.FindFirst("sub")?.Value;
            if (!String.IsNullOrWhiteSpace(sub))
            {
                if (Subjects.Contains(sub)) return true;
            }

            var roles = user.FindAll("role").Select(x => x.Value);
            if (roles.Any())
            {
                if (IdentityRoles.Any(x => roles.Contains(x))) return true;
            }

            return false;
        }
    }
}