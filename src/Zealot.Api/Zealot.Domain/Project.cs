using System.Collections.Generic;

namespace Zealot.Domain
{
    public class Project
    {
        public int EnvironmentId { get; set; }
        public List<Pack> Packs { get; set; }
    }
}
