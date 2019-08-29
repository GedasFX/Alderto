using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alderto.Web.Helpers
{
    // Due to number in JavaScript having an upper limit of size (2^53), send all UInt64 as strings.
    public class SnowflakeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ulong) == objectType || typeof(ulong?) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jt = JToken.ReadFrom(reader);
            return jt.Value<ulong>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}
