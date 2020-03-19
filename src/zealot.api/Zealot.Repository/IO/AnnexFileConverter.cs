using System.IO;
using System.Text;
using SystemWrap;
using YamlDotNet.Serialization;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public class AnnexFileConverter : IAnnexFileConverter
    {
        private readonly IFile _file;

        public AnnexFileConverter(IFile file)
        {
            _file = file;
        }

        public OpResult Dump<T>(T node, string path) where T : Node
        {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(CastObject<T>(node));
            _file.WriteAllText(path, yaml, Encoding.UTF8);
            return OpResult.Ok;
        }

        public OpResult<T> Read<T>(string path) where T : class
        {
            var deserializer = new DeserializerBuilder().Build();
            var requestNode = deserializer.Deserialize<T>(File.ReadAllText(path));
            return new OpResult<T>(true, requestNode);
        }
        public T CastObject<T>(object input)
        {
            return (T)input;
        }
    }
}
