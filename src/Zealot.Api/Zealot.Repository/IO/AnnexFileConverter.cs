using System.Collections.Generic;
using System.IO;
using System.Text;
using SystemWrap;
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
#warning SBU: Create constant for "httpMethod"
                var httpMethod = TryGetValue(subTree.Attributes, "httpMethod");
#warning SBU: Create constant for "requestUrl"
                var requestUrl = TryGetValue(subTree.Attributes, "requestUrl");

                _file.WriteAllText(Path.Combine(basePath, $"{subTree.Id}.req"), $"{httpMethod} {requestUrl}", Encoding.UTF8);
            }
            return OpResult.Ok;
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
