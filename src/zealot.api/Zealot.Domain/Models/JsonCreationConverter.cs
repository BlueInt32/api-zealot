using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zealot.Api.JsonIO
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        protected abstract T CreateObjectFromJson(Type objectType, JObject jObject);
        protected abstract JObject CreateJsonFromObject(T obj);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jObject = JObject.Load(reader);
            T target = CreateObjectFromJson(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            return target;
        }
    }
}
