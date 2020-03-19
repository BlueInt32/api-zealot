using System;
using Newtonsoft.Json;
using Zealot.Domain.Models;

namespace Zealot.Domain.Objects
{
    [JsonConverter(typeof(NodeJsonConverter))]
    public class Node
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
