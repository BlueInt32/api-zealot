using System.Collections.Generic;

namespace Zealot.Domain
{
    public class Pack
    {
        public List<Script> Scripts { get; set; }
        public List<Request> Requests { get; set; }
    }
}