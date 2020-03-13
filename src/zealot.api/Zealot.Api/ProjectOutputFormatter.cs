using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zealot.Domain;
using Zealot.Domain.Objects;
using Zealot.Domain.Utilities;

namespace Zealot.Api
{
    public class ProjectOutputFormatter : TextOutputFormatter
    {
        #region ctor
        public ProjectOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));

            SupportedEncodings.Add(Encoding.UTF8);
        }
        #endregion

        #region canwritetype
        protected override bool CanWriteType(Type type)
        {
            /// the output formatter should only be used with <see cref="Project"/> type
            if (typeof(Project).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }
        #endregion
        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (selectedEncoding == null)
            {
                throw new ArgumentNullException(nameof(selectedEncoding));
            }

            var response = context.HttpContext.Response;
            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                WriteObject(writer, context.Object);
                await writer.FlushAsync();
            }
        }

        /// <summary>
        /// When we want to serialize a Project object, we want the type of each node to be set in the output json.
        /// This has to be done manually because .net objects <see cref="PackNode"/>, <see cref="RequestNode"/>
        /// and <see cref="ScriptNode"/> define their type intrinsically, thus those types names aren't 
        /// written by default by the serializer.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        private void WriteObject(TextWriter writer, object value)
        {
            var typesMap = new Dictionary<Guid, string>();
            var project = value as Project; // type already checked by formatter, no need to null-check
            var serializer = new JsonSerializer();

            // 1. first we execute a traversal in the tree to populate a "nodeId -> type" map (typesMap)
            ProjectTreeHelper<Dictionary<Guid, string>>.ExecuteTraversal(
                project.Tree,
                (node, map) =>
                {
                    map.Add(node.Id, node.GetTypeConstant());
                }
                , typesMap);

            // 2. then using jsonPath we set the type property manually on each relevant jToken node
            var rawProject = JObject.FromObject(value);
            var tokens = rawProject.SelectTokens("$..*[?(@.id)]");
            foreach (var token in tokens)
            {
                token["type"] = typesMap[Guid.Parse(token["id"].ToString())];
            }

            serializer.Serialize(writer, rawProject);
        }
    }
}
