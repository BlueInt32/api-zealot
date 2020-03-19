using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Zealot.Domain.Exceptions;
using Zealot.Domain.Objects;

namespace Zealot.Domain.Models
{
    //public class ProjectJsonConverter : JsonCreationConverter<Project>
    //{
    //    public override bool CanWrite => false;

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override JObject CreateJsonFromObject(Project obj)
    //    {
    //        return JObject.FromObject(obj);
    //    }

    //    protected override Project CreateObjectFromJson(Type objectType, JObject jObject)
    //    {
    //        if (jObject == null) throw new ArgumentNullException("jObject");


    //        var resultProject = new Project();
    //        return resultProject;
    //    }
    //}

    public class NodeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Node).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject joe = JObject.Load(reader);
            var type = joe["type"].Value<string>();

            Node node;
            if (type == "request")
            {
                node = new RequestNode();
            }
            else if (type == "pack")
            {
                node = new PackNode();
            }
            else if (type == "script")
            {
                node = new ScriptNode();
            }
            else
            {
                throw new ZealotException("Could not deserialize node, unknown type");
            }
            serializer.Populate(joe.CreateReader(), node);
            return node;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            if (ReferenceEquals(value, null))
            {
                writer.WriteNull();
                return;
            }

            var contract = (JsonObjectContract)serializer
                .ContractResolver
                .ResolveContract(value.GetType());

            writer.WriteStartObject();

            writer.WritePropertyName("type");
            switch (value)
            {
                case PackNode pack:
                    writer.WriteValue("pack");
                    break;
                case RequestNode request:
                    writer.WriteValue("request");
                    break;
                case ScriptNode script:
                    writer.WriteValue("script");
                    break;
            }

            foreach (var property in contract.Properties)
            {
                if (property.Ignored) continue;
                if (!ShouldSerialize(property, value)) continue;

                var property_name = property.PropertyName;
                var property_value = property.ValueProvider.GetValue(value);

                writer.WritePropertyName(property_name);
                if (property.Converter != null && property.Converter.CanWrite)
                {
                    property.Converter.WriteJson(writer, property_value, serializer);
                }
                else
                {
                    serializer.Serialize(writer, property_value);
                }
            }

            writer.WriteEndObject();
        }

        private static bool ShouldSerialize(JsonProperty property, object instance)
        {
            return property.ShouldSerialize == null
                || property.ShouldSerialize(instance);
        }
    }
    //public static class JsonExtensions
    //{
    //    public static JToken DefaultFromObject(this JsonSerializer serializer, object value)
    //    {
    //        if (value == null)
    //            return JValue.CreateNull();
    //        var dto = Activator.CreateInstance(typeof(DefaultSerializationDTO<>).MakeGenericType(value.GetType()), value);
    //        var root = JObject.FromObject(dto, serializer);
    //        return root["Value"].RemoveFromLowestPossibleParent() ?? JValue.CreateNull();
    //    }

    //    public static object DefaultToObject(this JToken token, Type type, JsonSerializer serializer = null)
    //    {
    //        var oldParent = token.Parent;

    //        var dtoToken = new JObject(new JProperty("Value", token));
    //        var dtoType = typeof(DefaultSerializationDTO<>).MakeGenericType(type);
    //        var dto = (IHasValue)(serializer ?? JsonSerializer.CreateDefault()).Deserialize(dtoToken.CreateReader(), dtoType);

    //        if (oldParent == null)
    //            token.RemoveFromLowestPossibleParent();

    //        return dto == null ? null : dto.GetValue();
    //    }

    //    public static JToken RemoveFromLowestPossibleParent(this JToken node)
    //    {
    //        if (node == null)
    //            return null;
    //        // If the parent is a JProperty, remove that instead of the token itself.
    //        var contained = node.Parent is JProperty ? node.Parent : node;
    //        contained.Remove();
    //        // Also detach the node from its immediate containing property -- Remove() does not do this even though it seems like it should
    //        if (contained is JProperty)
    //            ((JProperty)node.Parent).Value = null;
    //        return node;
    //    }

    //    interface IHasValue
    //    {
    //        object GetValue();
    //    }

    //    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy), IsReference = false)]
    //    class DefaultSerializationDTO<T> : IHasValue
    //    {
    //        public DefaultSerializationDTO(T value) { this.Value = value; }

    //        public DefaultSerializationDTO() { }

    //        [JsonConverter(typeof(NoConverter)), JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Serialize)]
    //        public T Value { get; set; }

    //        object IHasValue.GetValue() { return Value; }
    //    }
    //}
    //public class NoConverter : JsonConverter
    //{
    //    // NoConverter taken from this answer https://stackoverflow.com/a/39739105/3744182
    //    // To https://stackoverflow.com/questions/39738714/selectively-use-default-json-converter
    //    // By https://stackoverflow.com/users/3744182/dbc
    //    public override bool CanConvert(Type objectType) { throw new NotImplementedException(); /* This converter should only be applied via attributes */ }

    //    public override bool CanRead { get { return false; } }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { throw new NotImplementedException(); }

    //    public override bool CanWrite { get { return false; } }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { throw new NotImplementedException(); }
    //}
}
