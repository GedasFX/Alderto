using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alderto.Web.Helpers
{
    // Due to number in JavaScript having an upper limit of size (2^53), send all UInt64 as strings.
    public class SnowflakeConverter : JsonConverter<ulong>
    {
        public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetUInt64();
        }

        public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}