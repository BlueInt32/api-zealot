using System.Text;
using Newtonsoft.Json;
using SystemWrap;
using Zealot.Domain.Utilities;

namespace Zealot.Repository.IO
{
    public class ObjectJsonDump<T> : IObjectJsonDump<T> where T : class
    {
        private readonly IFile _file;

        public ObjectJsonDump(IFile file)
        {
            _file = file;
        }
        public OpResult Dump(T inputObject, string path)
        {
            var json = JsonConvert.SerializeObject(inputObject, Formatting.Indented);
            _file.WriteAllText(path, json, Encoding.UTF8);
            return OpResult.Ok;
        }
    }
}
