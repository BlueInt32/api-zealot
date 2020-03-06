using System;
using System.Collections.Generic;
using Zealot.Domain.Enums;

namespace Zealot.Domain
{
    public class Node
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual TreeNodeType Type { get; set; }
        public virtual List<Node> Children { get; set; }
    }
}