using System.IO;
using System.Text;
using SystemWrap;
using YamlDotNet.Serialization;
using Zealot.Domain;
using Zealot.Domain.Enums;
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

        public OpResult Dump(Request subTree, string basePath)
        {
            if (subTree.Type == TreeNodeType.Request)
            {
                var serializer = new SerializerBuilder().Build();
                var yaml = serializer.Serialize(subTree);
                _file.WriteAllText(Path.Combine(basePath, $"{subTree.Id}.yml"), yaml, Encoding.UTF8);
            }
            return OpResult.Ok;
        }

        public OpResult<Request> Read(string path)
        {
            var deserializer = new DeserializerBuilder().Build();
            var requestNode = deserializer.Deserialize<Request>(File.ReadAllText(path));
            return new OpResult<Request>(true, requestNode);
        }
    }
}
