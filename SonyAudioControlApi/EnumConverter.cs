using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SonyAudioControlApi
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    internal sealed class EnumJsonStringValueAttribute : Attribute
    {
        public string StringValue { get; set; }

        public EnumJsonStringValueAttribute(string stringValue)
        {
            if (stringValue is null)
            {
                throw new ArgumentNullException("stringValue must not be null");
            }

            this.StringValue = stringValue;
        }
    }

    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    internal sealed class JsonEnumConverterAttribute : JsonConverterAttribute
    {
        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            return Activator.CreateInstance(typeof(EnumConverter<>).MakeGenericType(typeToConvert)) as JsonConverter;
        }
    }

    internal sealed class EnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct
    {
        private static Dictionary<Type, MappingConfiguration> _mappings = new Dictionary<Type, MappingConfiguration>();

        private struct MappingConfiguration
        {
            public bool MappingIsValid { get; set; }
            public Dictionary<string, TEnum> StringToEnumMapping { get; set; }
            public Dictionary<TEnum, string> EnumToStringMapping { get; set; }
        }


        private static MappingConfiguration tryGetMappingForType(Type type)
        {
            if (!EnumConverter<TEnum>._mappings.ContainsKey(type))
            {
                MappingConfiguration mapping = new MappingConfiguration()
                {
                    MappingIsValid = true,
                    EnumToStringMapping = new Dictionary<TEnum, string>(),
                    StringToEnumMapping = new Dictionary<string, TEnum>()
                };
                EnumConverter<TEnum>._mappings[type] = mapping;

                MemberInfo[] members = type.GetMembers();

                foreach (MemberInfo member in members)
                {
                    FieldInfo memberField = member as FieldInfo;
                    if (memberField != null && memberField.IsStatic && memberField.IsPublic)
                    {
                        EnumJsonStringValueAttribute attribute = member.GetCustomAttribute<EnumJsonStringValueAttribute>();
                        if (attribute == null)
                        {
                            // We've found an element without the attribute
                            mapping.MappingIsValid = false;
                        }
                        else
                        {
                            TEnum valueInstance = Enum.Parse<TEnum>(memberField.Name);
                            mapping.EnumToStringMapping.Add(valueInstance, attribute.StringValue);
                            mapping.StringToEnumMapping.Add(attribute.StringValue, valueInstance);
                        }
                    }
                }
            }

            return EnumConverter<TEnum>._mappings[type];
        }

        private static MappingConfiguration getMappingForType(Type type)
        {
            MappingConfiguration mapping = EnumConverter<TEnum>.tryGetMappingForType(type);
            if (!mapping.MappingIsValid)
            {
                throw new Exception($"Unable to initialize {type} mapping. A member is missing the EnumStringValueAttribute");
            }
            else
            {
                return mapping;
            }
        }

        public override bool CanConvert(Type type)
        {
            return tryGetMappingForType(type).MappingIsValid;
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            MappingConfiguration mapping = EnumConverter<TEnum>.getMappingForType(type);

            if (reader.TokenType == JsonTokenType.String)
            {
                return (TEnum)mapping.StringToEnumMapping.GetValueOrDefault(reader.GetString());
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new JsonException($"Unable to deserialize {reader.TokenType} into {type}");
            }
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            Type valueType = value.GetType();
            if (valueType.IsArray)
            {
                MappingConfiguration mapping = EnumConverter<TEnum>.getMappingForType(valueType.GetElementType());
                TEnum[] typedValueArray = value as TEnum[];
                if (typedValueArray != null)
                {
                    writer.WriteStartArray();
                    foreach (TEnum valueElement in typedValueArray)
                    {
                        writer.WriteStringValue(mapping.EnumToStringMapping[valueElement]);
                    }
                    writer.WriteEndArray();
                }
            }
            else
            {
                MappingConfiguration mapping = EnumConverter<TEnum>.getMappingForType(valueType);
                writer.WriteStringValue(mapping.EnumToStringMapping[value]);
            }
        }
    }
}
