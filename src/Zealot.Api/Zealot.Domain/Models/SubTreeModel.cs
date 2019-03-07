using System;
using System.Collections.Generic;
using Zealot.Domain.Enums;

namespace Zealot.Domain.Models
{
    public class SubTreeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TreeNodeType Type { get; set; }
        public List<SubTreeModel> Children { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
}
