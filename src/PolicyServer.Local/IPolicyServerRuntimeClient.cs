// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Security.Claims;
using System.Threading.Tasks;

namespace PolicyServer.Runtime.Client
{
    /// <summary>
    /// Interface for PolicyServer client
    /// </summary>
    public interface IPolicyServerRuntimeClient
    {
        /// <summary>
        /// Evaluates the policy for a given user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task<PolicyResult> EvaluateAsync(ClaimsPrincipal user);

        /// <summary>
        /// Determines whether the user has a permission.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permission">The permission.</param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);

        /// <summary>
        /// Determines whether the user is in a role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Task<bool> IsInRoleAsync(ClaimsPrincipal user, string role);
    }
}