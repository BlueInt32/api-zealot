using System;
using Newtonsoft.Json;

namespace Zealot.Domain.Objects
{
    public class Node
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
