using System;
using System.Collections.Generic;
using Zealot.Domain.Objects;

namespace Zealot.Domain
{
    public class RequestNode : INode
    {
        public string EndpointUrl { get; set; }
        public string HttpMethod { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}