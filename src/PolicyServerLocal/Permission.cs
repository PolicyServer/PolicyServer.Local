using System;
using System.Collections.Generic;
using System.Linq;

namespace PolicyServerLocal
{
    public class Permission
    {
        public string Name { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        public bool Evaluate(IEnumerable<string> roles)
        {
            if (roles == null) throw new ArgumentNullException(nameof(roles));

            if (Roles.Any(x => roles.Contains(x))) return true;

            return false;
        }
    }
}