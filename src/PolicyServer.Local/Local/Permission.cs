// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace PolicyServer.Local
{
    public class Permission
    {
        public string Name { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        internal bool Evaluate(IEnumerable<string> roles)
        {
            if (roles == null) throw new ArgumentNullException(nameof(roles));

            if (Roles.Any(x => roles.Contains(x))) return true;

            return false;
        }
    }
}