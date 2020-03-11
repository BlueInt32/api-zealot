using System.IO;
using System.Text;
using SystemWrap;
using YamlDotNet.Serialization;
using Zealot.Domain;
using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public class RequestFileConverter : IRequestFileConverter
    {
        private readonly IFile _file;

        public RequestFileConverter(IFile file)
        {
            _file = file;
        }

        public OpResult Dump(RequestNode request, string basePath)
        {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(request);
            _file.WriteAllText(Path.Combine(basePath, $"{request.Id}.yml"), yaml, Encoding.UTF8);
            return OpResult.Ok;
        }

        public OpResult<RequestNode> Read(string path)
        {
            var deserializer = new DeserializerBuilder().Build();
            var requestNode = deserializer.Deserialize<RequestNode>(File.ReadAllText(path));
            return new OpResult<RequestNode>(true, requestNode);
        }
    }
}
