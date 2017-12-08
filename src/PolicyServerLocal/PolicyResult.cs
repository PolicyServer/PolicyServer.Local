using System.Collections.Generic;

namespace PolicyServerLocal
{
    public class PolicyResult
    {
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}