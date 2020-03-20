using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Zealot.Domain.Exceptions;
using Zealot.Domain.Objects;

namespace Zealot.Domain.Utilities
{
    public class NodeJsonConverter : JsonConverter
    {
        private readonly OutputDetailsLevel outputDetailsLevel;

        public NodeJsonConverter()
        {
            this.outputDetailsLevel = OutputDetailsLevel.NodeLeaf;
        }
        public NodeJsonConverter(OutputDetailsLevel outputDetailsLevel)
        {
            this.outputDetailsLevel = outputDetailsLevel;
        }
        private List<string> _allowedProjectLevelProperties
        {
            get
            {
                switch (outputDetailsLevel)
                {
                    case OutputDetailsLevel.ProjectStructure:
                        return new List<string> { "id", "name", "type", "children" };
                    case OutputDetailsLevel.NodeLeaf:
                        return new List<string> { "id", "name", "type", "children", "endpointUrl", "httpMethod", "code" };
                }
                return new List<string>();
            }
        }

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
                if (!_allowedProjectLevelProperties.Contains(property.PropertyName)
                    || property.Ignored
                    || !ShouldSerialize(property, value))
                {
                    continue;
                }

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
}
