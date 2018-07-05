// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace PolicyServer.Runtime.Client
{
    /// <summary>
    /// The result of a policy evaluation
    /// </summary>
    public class PolicyResult
    {
        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public IEnumerable<string> Permissions { get; set; }
    }
}
