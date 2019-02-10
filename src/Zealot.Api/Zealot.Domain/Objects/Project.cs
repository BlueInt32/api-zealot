using System.Collections.Generic;

namespace Zealot.Domain.Objects
{
    public class Project
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public int? EnvironmentId { get; set; }
        public List<Pack> Packs { get; set; }
        public static Project CreateDefaultInstance()
        {
            return new Project
            {
                Packs = new List<Pack>
                {
                    new Pack
                    {
                        Requests = new List<Request>
                        {
                            new Request
                            {
                                EndpointUrl = "http://localhost",
                                Headers = new Dictionary<string, string>
                                {
                                    { "Content type", "application/json" }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
