using System;

namespace Zealot.Domain.Models
{
    public class ProjectModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public PacksTreeModel PacksTree { get; set; }
    }
}
