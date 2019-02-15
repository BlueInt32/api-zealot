using System.Collections.Generic;
using Zealot.Domain.Enums;

namespace Zealot.Domain
{
    public class SubTree
    {
        public string Name { get; set; }
        public TreeNodeType Type { get; set; }
        public virtual List<SubTree> Children { get; set; }
    }
}