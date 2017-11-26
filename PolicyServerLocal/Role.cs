using System.Collections.Generic;

namespace PolicyServerLocal
{
    public class Role
    {
        public string Name { get; set; }

        public List<string> Members { get; set; }
        public List<string> Permissions { get; set; }
    }
}