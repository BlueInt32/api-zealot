using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Zealot.Domain.Enums;

namespace Zealot.Domain
{
    public class SubTree
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual TreeNodeType Type { get; set; }
        public virtual List<SubTree> Children { get; set; }
        [JsonIgnore]
        public Dictionary<string, string> Attributes { get; set; }
    }
}