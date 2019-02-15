using System.Collections.Generic;
using Zealot.Domain.Enums;

namespace Zealot.Domain.Models
{
    public class SubTreeModel
    {
        public string Name { get; set; }
        public TreeNodeType Type { get; set; }
        public List<SubTreeModel> Children { get; set; }
    }
}
