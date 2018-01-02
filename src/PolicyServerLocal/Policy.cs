using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PolicyServerLocal
{
    public class Policy
    {
        public List<Role> Roles { get; set; } = new List<Role>();
        public List<Permission> Permissions { get; set; } = new List<Permission>();

        public PolicyResult Evaluate(ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var roles = Roles.Where(x=>x.Evaluate(user)).Select(x=>x.Name).ToArray();
            var permissions = Permissions.Where(x => x.Evaluate(roles)).Select(x => x.Name).ToArray();

            var result = new PolicyResult()
            {
                Roles = roles.Distinct(),
                Permissions = permissions.Distinct()
            };

            return result;
        }
    }
}
