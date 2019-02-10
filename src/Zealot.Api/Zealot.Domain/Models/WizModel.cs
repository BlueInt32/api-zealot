namespace Zealot.Domain.Models
{
    public class WizModel
    {
        public HttpMethod HttpMethod { get; set; }
        public string EndpointUrl { get; set; }
        public string Body { get; set; }
    }
}
