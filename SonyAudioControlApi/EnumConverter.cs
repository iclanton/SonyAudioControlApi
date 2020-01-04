using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SonyAudioControlApi
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    internal sealed class EnumStringValueAttribute : Attribute
    {
        public string StringValue { get; set; }

        public EnumStringValueAttribute(string stringValue)
        {
            if (stringValue is null)
            {
                throw new ArgumentNullException("stringValue must not be null");
            }

            this.StringValue = stringValue;
        }
    }

    internal sealed class EnumConverter<TEnum> : JsonConverter where TEnum : struct
    {
        private static bool _mappingsHaveBeenInitialized = false;
        private static Dictionary<string, TEnum> _stringToEnumMapping;
        private static Dictionary<TEnum, string> _enumToStringMapping;

        private static void initilizeMappings()
        {
            if (!EnumConverter<TEnum>._mappingsHaveBeenInitialized)
            {
                EnumConverter<TEnum>._enumToStringMapping = new Dictionary<TEnum, string>();
                EnumConverter<TEnum>._stringToEnumMapping = new Dictionary<string, TEnum>();

                Type type = typeof(TEnum);
                MemberInfo[] members = type.GetMembers();

                foreach (MemberInfo member in members)
                {
                    FieldInfo memberField = member as FieldInfo;
                    if (memberField != null && memberField.IsStatic && memberField.IsPublic)
                    {
                        EnumStringValueAttribute attribute = member.GetCustomAttribute<EnumStringValueAttribute>();
                        if (attribute == null)
                        {
                            throw new Exception($"Unable to initialize {type} mappings. {memberField.Name} is missing the EnumStringValueAttribute");
                        }
                        else
                        {
                            TEnum valueInstance = Enum.Parse<TEnum>(memberField.Name);
                            EnumConverter<TEnum>._enumToStringMapping.Add(valueInstance, attribute.StringValue);
                            EnumConverter<TEnum>._stringToEnumMapping.Add(attribute.StringValue, valueInstance);
                        }
                    }
                }

                EnumConverter<TEnum>._mappingsHaveBeenInitialized = true;
            }

        }

        private static Dictionary<string, TEnum> stringToEnumMapping
        {
            get
            {
                EnumConverter<TEnum>.initilizeMappings();
                return EnumConverter<TEnum>._stringToEnumMapping;
            }
        }

        private static Dictionary<TEnum, string> enumToStringMapping
        {
            get
            {
                EnumConverter<TEnum>.initilizeMappings();
                return EnumConverter<TEnum>._enumToStringMapping;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
            {
                return EnumConverter<TEnum>.stringToEnumMapping.GetValueOrDefault(reader.Value as string);
            }
            else if (reader.ValueType == typeof(string[]))
            {
                return (reader.Value as string[]).Select(EnumConverter<TEnum>.stringToEnumMapping.GetValueOrDefault).ToArray();
            }
            else
            {
                throw new JsonSerializationException($"Unable to deserialize {reader.ValueType} into {typeof(TEnum)}");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TEnum? typedValue = value as TEnum?;
            if (typedValue != null)
            {
                writer.WriteValue(EnumConverter<TEnum>.enumToStringMapping[typedValue ?? default(TEnum)]);
            }
            else
            {
                TEnum[] typedValueArray = value as TEnum[];
                if (typedValueArray != null)
                {
                    writer.WriteStartArray();
                    foreach (TEnum? valueElement in typedValueArray)
                    {
                        writer.WriteValue(EnumConverter<TEnum>.enumToStringMapping[valueElement ?? default(TEnum)]);
                    }
                    writer.WriteEndArray();
                }
                else
                {
                    throw new JsonSerializationException($"Unable to serialize {value} of type {typeof(TEnum)} into string");
                }
            }
        }
    }
}
