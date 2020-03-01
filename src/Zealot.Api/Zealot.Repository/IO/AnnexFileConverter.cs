using System.Collections.Generic;
using System.IO;
using System.Text;
using SystemWrap;
using YamlDotNet.Serialization;
using Zealot.Domain;
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

        public OpResult Dump(SubTree subTree, string basePath)
        {
            if (subTree.Type == Domain.Enums.TreeNodeType.Request)
            {
                var httpMethod = TryGetValue(subTree.Attributes, Constants.RequestAttributes.HttpMethod);
                var requestUrl = TryGetValue(subTree.Attributes, Constants.RequestAttributes.RequestUrl);

                var serializer = new SerializerBuilder().Build();
                var yaml = serializer.Serialize(subTree);
                _file.WriteAllText(Path.Combine(basePath, $"{subTree.Id}.yml"), yaml, Encoding.UTF8);
            }
            return OpResult.Ok;
        }

        public OpResult<SubTree> Read(string path)
        {
            var deserializer = new DeserializerBuilder().Build();
            var subTree = deserializer.Deserialize<SubTree>(path);
            return new OpResult<SubTree>(true, subTree);
        }

        private string TryGetValue(Dictionary<string, string> attributes, string key)
        {
            if (!attributes.ContainsKey(key))
            {
                return string.Empty;
            }
            return attributes[key];
        }
    }
}
