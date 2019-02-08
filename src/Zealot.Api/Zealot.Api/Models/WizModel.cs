using Zealot.Api.Domain;

namespace Zealot.Api.Models
{
    public class WizModel
    {
        public HttpVerbEnum Verb { get; set; }
        public string Endpoint { get; set; }
        public string Body { get; set; }
    }
}
