using System;
using System.Collections.Generic;

namespace Zealot.Domain.Objects
{
    public class Project
    {
        public Guid Id { get; set; }
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
