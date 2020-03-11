using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SystemWrap;
using Zealot.Domain;
using Zealot.Domain.Exceptions;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public class JsonFileConverter<T> : IJsonFileConverter<T> where T : class
    {
        private readonly IFile _file;

        public JsonFileConverter(IFile file)
        {
            _file = file;
        }
        public OpResult Dump(T inputObject, string path)
        {
            JsonConvert.DefaultSettings = () =>
            {
                return new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            };
            var json = JsonConvert.SerializeObject(inputObject, Formatting.Indented, new StringEnumConverter(typeof(CamelCaseNamingStrategy)));
            _file.WriteAllText(path, json, Encoding.UTF8);
            return OpResult.Ok;
        }

        public OpResult<T> Read(string path)
        {
            if (!_file.Exists(path))
            {
                return OpResult<T>.Bad(Domain.ErrorCode.FILE_DOES_NOT_EXIST, $"File at {path} does not exist");
            }
            var fileContent = _file.ReadAllText(path, Encoding.UTF8);
            //var outputObject = JsonConvert.DeserializeObject<T>(fileContent);
            var outputObject = JsonConvert.DeserializeObject<T>(fileContent,
                new NodeConverter());
            return new OpResult<T>(true, outputObject);
        }
    }

    public class NodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(INode).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            bool isPackNode = jo["type"].ToString() == "pack";
            bool isRequestNode = jo["type"].ToString() == "request";
            bool isScriptNode = jo["type"].ToString() == "code";
            INode item;
            if (isPackNode)
                item = new PackNode();
            else if (isRequestNode)
                item = new RequestNode();
            else if (isScriptNode)
                item = new ScriptNode();
            else
                throw new ZealotException("UNKNOWN_NODE_TYPE");

            serializer.Populate(jo.CreateReader(), item);

            return item;

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
