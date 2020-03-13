using System;
using Newtonsoft.Json;
using Zealot.Domain.Objects;

namespace Zealot.Domain
{
    public class ScriptNode : INode
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

    }
}