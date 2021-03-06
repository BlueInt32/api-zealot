﻿using Zealot.Domain.Objects;

namespace Zealot.Repository.Utils
{
    public class NodeRecursionContext
    {
        public Project Project { get; set; }
        public Node CurrentNode { get; set; }
        public Node ParentNode { get; set; }
        public int ChildIndex { get; set; }

    }
}
