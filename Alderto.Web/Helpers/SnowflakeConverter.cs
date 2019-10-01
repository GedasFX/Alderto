using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alderto.Web.Helpers
{
    // Due to number in JavaScript having an upper limit of size (2^53), send all UInt64 as strings.

    public class SnowflakeConverter : JsonConverter<ulong>
    {
        public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

                if (Utf8Parser.TryParse(span, out ulong number, out var bytesConsumed) && span.Length == bytesConsumed)
                    return number;

                if (ulong.TryParse(reader.GetString(), out number))
                    return number;
            }

            return reader.GetUInt64();
        }

        public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    public class NullableSnowflakeConverter : JsonConverter<ulong?>
    {
        public override ulong? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

                if (Utf8Parser.TryParse(span, out ulong number, out var bytesConsumed) && span.Length == bytesConsumed)
                    return number;

                if (ulong.TryParse(reader.GetString(), out number))
                    return number;
            }

            if (reader.TryGetUInt64(out var snowflake))
                return snowflake;

            return null;
        }

        public override void Write(Utf8JsonWriter writer, ulong? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString());
        }
    }
}