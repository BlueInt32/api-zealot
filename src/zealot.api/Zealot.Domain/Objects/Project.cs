using System;
using System.Collections.Generic;

namespace Zealot.Domain.Objects
{
    public class Project
    {
        public Guid Id { get; set; }
        public int? EnvironmentId { get; set; }
        public Node Tree { get; set; }
        public static Project CreateDefaultInstance()
        {
            return new Project
            {
                Tree = new Node
                {
                    Children = new List<Node>
                    {
                        new Request
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
