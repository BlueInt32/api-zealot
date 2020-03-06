using System.Collections.Generic;
using Zealot.Domain.Enums;

namespace Zealot.Domain
{
    public class Request : Node
    {
        public override TreeNodeType Type
        {
            get { return TreeNodeType.Request; }
            set { return; }
        }
        public string EndpointUrl { get; set; }
        public string HttpMethod { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public override List<Node> Children
        {
            get
            {
                return null;
            }
            set
            {
                return;
            }
        }
    }
}