// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace PolicyServer.Client
{
    public class PolicyResult
    {
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}