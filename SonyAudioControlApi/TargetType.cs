using System;
using System.Collections.Generic;
using System.Text;

namespace SonyAudioControlApi
{

    [JsonEnumConverter]
    public enum TargetType
    {
        /// <summary>
        /// Type information is unavailable.
        /// </summary>
        [EnumJsonStringValue("")]
        Unknown,

        /// <summary>
        /// A Boolean type containing only two values. For example: "off" and "on", or "false" and "true".
        /// </summary>
        [EnumJsonStringValue("booleanTarget")]
        BooleanTarget,

        /// <summary>
        /// A number type, including floating point numbers. For example: "1.5", "-10.0".
        /// </summary>
        [EnumJsonStringValue("doubleNumberTarget")]
        DoubleNumberTarget,

        /// <summary>
        /// An enumeration type containing a finite set of values. For example: "high", "mid", "low".
        /// </summary>
        [EnumJsonStringValue("enumTarget")]
        EnumTarget,

        /// <summary>
        /// An integer type. For example: "1", "-10".
        /// </summary>
        [EnumJsonStringValue("integerTarget")]
        IntegerTarget,

        /// <summary>
        /// A string type. For example: "hello".
        /// </summary>
        [EnumJsonStringValue("stringTarget")]
        StringTarget
    }
}
