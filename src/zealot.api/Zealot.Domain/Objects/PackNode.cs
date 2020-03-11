using System;
using System.Collections.Generic;
using Zealot.Domain.Objects;

namespace Zealot.Domain
{
    public class PackNode : INode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<INode> Children { get; set; }
    }
}