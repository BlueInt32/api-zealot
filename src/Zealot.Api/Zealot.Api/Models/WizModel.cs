using RestSharp;

namespace Zealot.Api.Models
{
    public class WizModel
    {
        public Method HttpVerb { get; set; }
        public string EndpointUrl { get; set; }
        public string Body { get; set; }
    }
}
