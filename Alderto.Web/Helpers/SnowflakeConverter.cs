using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alderto.Web.Helpers
{
    // Due to number in JavaScript having an upper limit of size (2^53), send all UInt64 as strings.
    public class SnowflakeConverter<T> : JsonConverter<T>
    {
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return JToken.ReadFrom(reader).Value<T>();
        }
    }
}