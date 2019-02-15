using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SystemWrap;
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
            var outputObject = JsonConvert.DeserializeObject<T>(fileContent);
            return new OpResult<T>(true, outputObject);
        }
    }
}
