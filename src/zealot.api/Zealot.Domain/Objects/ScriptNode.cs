using System;
using Zealot.Domain.Objects;

namespace Zealot.Domain
{
    public class ScriptNode : INode
    {
        public string Code { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}