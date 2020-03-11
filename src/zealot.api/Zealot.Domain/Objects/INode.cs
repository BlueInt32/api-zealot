using System;

namespace Zealot.Domain.Objects
{
    public interface INode
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }
}
