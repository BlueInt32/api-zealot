using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Zealot.Domain.Objects;

namespace Zealot.Domain
{
    public class RequestNode : INode
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("endpointUrl")]
        public string EndpointUrl { get; set; }

        [JsonProperty("httpMethod")]
        public string HttpMethod { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }
    }
}