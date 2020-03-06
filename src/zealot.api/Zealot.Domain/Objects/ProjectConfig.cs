using System;
using Newtonsoft.Json;

namespace Zealot.Domain.Objects
{
    public class ProjectConfig
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
