using System.Collections.Generic;

namespace Zealot.Domain
{
    public class Request : SubTree
    {
        public string EndpointUrl { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public override List<SubTree> Children
        {
            get
            {
                return null;
            }
        }
    }
}