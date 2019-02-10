using System.Collections.Generic;

namespace Zealot.Domain
{
    public class Request
    {
        public string EndpointUrl { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}