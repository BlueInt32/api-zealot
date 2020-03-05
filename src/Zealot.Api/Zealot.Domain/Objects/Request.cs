using System.Collections.Generic;
using Zealot.Domain.Enums;
using Zealot.Domain.Exceptions;

namespace Zealot.Domain
{
    public class Request : SubTree
    {
        public override TreeNodeType Type
        {
            get { return TreeNodeType.Request; }
            set { throw new ZealotException("Cannot write Request type, it is already set !"); }
        }
        public string EndpointUrl { get; set; }
        public string HttpMethod { get; set; }
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