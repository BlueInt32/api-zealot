using System.Collections.Generic;
using Zealot.Domain.Enums;

namespace Zealot.Domain.Models
{
    public class PacksTreeModel
    {
        public TreeNodeType Type { get; set; }
        public List<PacksTreeModel> Children { get; set; }
    }
}
