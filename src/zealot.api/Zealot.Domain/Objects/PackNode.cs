using System.Collections.Generic;
using Newtonsoft.Json;
using Zealot.Domain.Objects;

namespace Zealot.Domain
{
    public class PackNode : Node
    {
        [JsonProperty("children")]
        public virtual List<Node> Children { get; set; }
    }
}