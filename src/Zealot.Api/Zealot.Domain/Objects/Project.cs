using System;
using System.Collections.Generic;

namespace Zealot.Domain.Objects
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public int? EnvironmentId { get; set; }
        public PacksTree PacksTree { get; set; }
        public static Project CreateDefaultInstance()
        {
            return new Project
            {
                PacksTree = new PacksTree
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
            };
        }
    }
}
