using System.Collections.Generic;
using Zealot.Domain.Enums;

namespace Zealot.Domain
{
    public class PacksTree
    {
        public TreeNodeType Type { get; set; }
        public List<PacksTree> Children { get; set; }
        public List<Script> Scripts { get; set; }
        public List<Request> Requests { get; set; }
    }
}