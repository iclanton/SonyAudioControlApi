using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SonyAudioControlApi
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal sealed class SingleElementArrayConverterAttribute : JsonConverterAttribute
    {
        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            return Activator.CreateInstance(typeof(SingleElementArrayConverter<>).MakeGenericType(typeToConvert)) as JsonConverter;
        }
    }

    internal sealed class SingleElementArrayConverter<TElement> : JsonConverter<TElement>
    {
        public override bool CanConvert(Type type)
        {
            return true;
        }

        public override TElement Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();
                TElement value = JsonSerializer.Deserialize<TElement>(ref reader, options);
                reader.Read();
                if (reader.TokenType != JsonTokenType.EndArray)
                {
                    throw new JsonException($"Expected to find a single-eleemt array, but found an unexpected element: {reader.TokenType}");
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return JsonSerializer.Deserialize<TElement>(ref reader, options);
            }
        }

        public override void Write(Utf8JsonWriter writer, TElement value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            JsonSerializer.Serialize<TElement>(value, options);
            writer.WriteEndArray();
        }
    }
}
