using System;
using System.Collections.Generic;
using System.Text;

namespace SonyAudioControlApi
{
    [JsonEnumConverter]
    internal enum ApiVersion
    {
        [EnumJsonStringValue("1.0")]
        V10,

        [EnumJsonStringValue("1.1")]
        V11,

        [EnumJsonStringValue("1.2")]
        V12
    }
}
