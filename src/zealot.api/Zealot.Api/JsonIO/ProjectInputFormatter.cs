using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Zealot.Domain.Objects;

namespace Zealot.Api.JsonIO
{
    public class ProjectInputFormatter : TextInputFormatter
    {
        public ProjectInputFormatter()
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
        }


        protected override bool CanReadType(Type type)
        {
            if (type == typeof(Project))
            {
                return base.CanReadType(type);
            }
            return false;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var request = context.HttpContext.Request;



            using (var reader = new StreamReader(request.Body, encoding))
            {
                var x = new JsonSerializer();
                var resultProject = (Project)x.Deserialize(reader, typeof(Project));
                return await InputFormatterResult.SuccessAsync(resultProject);
                //try
                //{
                //    await ReadLineAsync("BEGIN:VCARD", reader, context);
                //    await ReadLineAsync("VERSION:2.1", reader, context);

                //    var nameLine = await ReadLineAsync("N:", reader, context);
                //    var split = nameLine.Split(";".ToCharArray());
                //    var contact = new Contact() { LastName = split[0].Substring(2), FirstName = split[1] };

                //    await ReadLineAsync("FN:", reader, context);

                //    var idLine = await ReadLineAsync("UID:", reader, context);
                //    contact.ID = idLine.Substring(4);

                //    await ReadLineAsync("END:VCARD", reader, context);

                //    return await InputFormatterResult.SuccessAsync(contact);
                //}
                //catch
                //{
                //    return await InputFormatterResult.FailureAsync();
                //}
            }
        }
        private async Task<string> ReadLineAsync(string expectedText, StreamReader reader, InputFormatterContext context)
        {
            var line = await reader.ReadLineAsync();
            if (!line.StartsWith(expectedText))
            {
                var errorMessage = $"Looked for '{expectedText}' and got '{line}'";
                context.ModelState.TryAddModelError(context.ModelName, errorMessage);
                throw new Exception(errorMessage);
            }
            return line;
        }
    }
}
