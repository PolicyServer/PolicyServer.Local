using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PolicyServerLocal.Tests
{
    public class TestUser
    {
        public static ClaimsPrincipal Create(string sub, 
            IEnumerable<string> roles = null, IEnumerable<Claim> claims = null)
        {
            var list = new List<Claim>();
            list.Add(new Claim("sub", sub));

            if (roles != null)
            {
                list.AddRange(roles.Select(x => new Claim("role", x)));
            }

            if (claims != null)
            {
                list.AddRange(claims);
            }

            var ci = new ClaimsIdentity(list, "pwd", "name", "role");
            var cp = new ClaimsPrincipal(ci);

            return cp;
        }
    }
}
