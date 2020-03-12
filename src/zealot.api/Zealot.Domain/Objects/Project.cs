using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zealot.Domain.Objects
{
    public class Project
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public int? EnvironmentId { get; set; }

        public PackNode Tree { get; set; }

        public static Project CreateDefaultInstance()
        {
            return new Project
            {
                Tree = new PackNode
                {
                    Children = new List<INode>
                    {
                        new RequestNode
                        {
                            EndpointUrl = "http://localhost:9500",
                            Headers = new Dictionary<string, string>
                            {
                                { "Content type", "application/json" }
                            }
                        }
                    }
                }
            };
        }
    }
}
