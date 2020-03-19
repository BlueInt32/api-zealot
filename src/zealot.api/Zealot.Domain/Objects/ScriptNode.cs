using Newtonsoft.Json;
using Zealot.Domain.Objects;

namespace Zealot.Domain
{
    public class ScriptNode : Node
    {
        [JsonProperty("code")]
        public string Code { get; set; }

    }
}