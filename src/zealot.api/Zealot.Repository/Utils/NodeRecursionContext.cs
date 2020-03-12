using Zealot.Domain.Objects;

namespace Zealot.Repository.Utils
{
    public class NodeRecursionContext
    {
        public Project Project { get; set; }
        public INode CurrentNode { get; set; }
        public INode ParentNode { get; set; }
        public int ChildIndex { get; set; }

    }
}
