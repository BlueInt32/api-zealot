using System;

namespace Zealot.Domain.Models
{
    public class ProjectModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public SubTreeModel Tree { get; set; }
    }
}
