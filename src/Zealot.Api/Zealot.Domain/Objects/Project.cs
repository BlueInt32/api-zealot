using System;
using System.Collections.Generic;

namespace Zealot.Domain.Objects
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? EnvironmentId { get; set; }
        public SubTree Tree { get; set; }
        public static Project CreateDefaultInstance()
        {
            return new Project
            {
                Tree = new SubTree
                {
                    Name = "Root",
                    Children = new List<SubTree>
                    {
                        new Request
                        {
                            Name = "Get ES root",
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
