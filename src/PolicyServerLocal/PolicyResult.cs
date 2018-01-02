using System.Collections.Generic;

namespace PolicyServer.Client
{
    public class PolicyResult
    {
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}