using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Zealot.Domain.Objects;

namespace Zealot.Domain
{
    public class PackNode : INode
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("children")]
        public virtual List<INode> Children { get; set; }
    }
}