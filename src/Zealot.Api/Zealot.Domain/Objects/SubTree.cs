using System;
using System.Collections.Generic;
using Zealot.Domain.Enums;

namespace Zealot.Domain
{
    public class SubTree
    {
        public Guid Id
        {
            get
            {
                return Guid.NewGuid();
            }
        }
        public string Name { get; set; }
        public TreeNodeType Type { get; set; }
        public virtual List<SubTree> Children { get; set; }
    }
}