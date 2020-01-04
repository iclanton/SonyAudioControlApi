using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        /// <summary>
        /// Gets information about the current custom equalizer settings.
        /// </summary>
        public async Task<CustomEqualizerSettingsResult[]> GetCustomEqualizerSettingsAsync(CustomEqualizerSettingsResult.EqualizerTarget target = CustomEqualizerSettingsResult.EqualizerTarget.AllTargets)
        {
            return await ApiRequest.MakeRequestAsync<CustomEqualizerSettingsResult[]>(
                this.Device,
                ApiLib.Audio,
                ApiVersion.V10,
                "getCustomEqualizerSettings",
                new CustomEqualizerSettingsProps() { Target  = target }
            );
        }
    }

    internal sealed class CustomEqualizerSettingsProps
    {
        [JsonPropertyName("target")]
        public CustomEqualizerSettingsResult.EqualizerTarget Target { get; set; }
    }

    public sealed class CustomEqualizerSettingsResult
    {
        [JsonEnumConverter]
        public enum EqualizerTarget
        {
            /// <summary>
            /// All equalizer settings.
            /// </summary>
            [EnumJsonStringValue("")]
            AllTargets,

            /// <summary>
            /// The level for the 100 Hz band in the equalizer.
            /// </summary>
            [EnumJsonStringValue("100HzBandLevel")]
            X100HzBandLevel,

            /// <summary>
            /// The level for the 330 Hz band in the equalizer.
            /// </summary>
            [EnumJsonStringValue("330HzBandLevel")]
            X330HzBandLevel,

            /// <summary>
            /// The level of the 1,000 Hz band in the equalizer.
            /// </summary>
            [EnumJsonStringValue("1000HzBandLevel")]
            X1000HzBandLevel,

            /// <summary>
            /// The level of the 3,300 Hz band in the equalizer.
            /// </summary>
            [EnumJsonStringValue("3300HzBandLevel")]
            X3300HzBandLevel,

            /// <summary>
            /// The level of the 10,000 Hz band in the equalizer.
            /// </summary>
            [EnumJsonStringValue("10000HzBandLevel")]
            X10000HzBandLevel,

            /// <summary>
            /// The level of the front bass in the equalizer.
            /// </summary>
            [EnumJsonStringValue("frontBassLevel")]
            FrontBassLevel,

            /// <summary>
            /// The level of the front treble in the equalizer.
            /// </summary>
            [EnumJsonStringValue("frontTrebleLevel")]
            FrontTrebleLevel,

            /// <summary>
            /// The level of the center bass in the equalizer.
            /// </summary>
            [EnumJsonStringValue("centerBassLevel")]
            CenterBassLevel,

            /// <summary>
            /// The level of the center treble in the equalizer.
            /// </summary>
            [EnumJsonStringValue("centerTrebleLevel")]
            CenterTrebleLevel,

            /// <summary>
            /// The level of the surround bass in the equalizer.
            /// </summary>
            [EnumJsonStringValue("surroundBassLevel")]
            SurroundBassLevel,

            /// <summary>
            /// The level of the surround treble in the equalizer.
            /// </summary>
            [EnumJsonStringValue("surroundTrebleLevel")]
            SurroundTrebleLevel,

            /// <summary>
            /// The level of the front high bass in the equalizer.
            /// </summary>
            [EnumJsonStringValue("frontHighBassLevel")]
            FrontHighBassLevel,

            /// <summary>
            /// The level of the front high treble in the equalizer.
            /// </summary>
            [EnumJsonStringValue("frontHighTrebleLevel")]
            FrontHighTrebleLevel,

            /// <summary>
            /// The level of the bass in the equalizer.
            /// </summary>
            [EnumJsonStringValue("bassLevel")]
            BassLevel,

            /// <summary>
            /// The level of the treble in the equalizer.
            /// </summary>
            [EnumJsonStringValue("trebleLevel")]
            TrebleLevel,

            /// <summary>
            /// The level of the bass in the equalizer.
            /// </summary>
            [EnumJsonStringValue("heightBassLevel")]
            HeightBassLevel,

            /// <summary>
            /// The level of the treble in the equalizer.
            /// </summary>
            [EnumJsonStringValue("heightTrebleLevel")]
            HeightTrebleLevel
        }

        [JsonEnumConverter]
        public enum EqualizerTargetType
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

        public sealed class EqualizerCandidate
        {
            /// <summary>
            /// Indicates whether the equalizer setting is currently available.
            /// </summary>
            [JsonPropertyName("isAvailable")]
            public bool IsAvailable { get; set; }

            /// <summary>
            /// The maximum value of the equalizer setting, or -1 if the value type is non-numeric.
            /// </summary>
            [JsonPropertyName("max")]
            public double Max { get; set; }

            /// <summary>
            /// The minimum value of the equalizer setting, or -1 if the value type is non-numeric.
            /// </summary>
            [JsonPropertyName("min")]
            public double Min { get; set; }

            /// <summary>
            /// The step value of the equalizer setting, or -1 if the value type is non-numeric.
            /// </summary>
            [JsonPropertyName("step")]
            public double Step { get; set; }

            /// <summary>
            /// The display title for the equalizer setting. "" indicates that this setting has no assigned title.
            /// </summary>
            [JsonPropertyName("title")]
            public string Title { get; set; }

            /// <summary>
            /// The product-specific identifier that the services uses to identify the equalizer setting.
            /// "" indicates that this setting has no assigned identifier.
            /// </summary>
            [JsonPropertyName("titleTextID")]
            public string TitleTextID { get; set; }

            /// <summary>
            /// The current value of the equalizer setting. If this property is "" or omitted, then the
            /// current value of the setting is an integer in a defined range with a fixed step.
            /// </summary>
            [JsonPropertyName("value")]
            public string Value { get; set; }
        }

        /// <summary>
        /// Additional information about the equalizer setting. If the equalizer setting is not available on this
        /// audio product, then this property will be null.
        /// </summary>
        [JsonPropertyName("candidate")]
        [SingleElementArrayConverter]
        public EqualizerCandidate Candidate { get; set; }

        /// <summary>
        /// The current value of the equalizer setting. The value is unitless and device dependent.
        /// </summary>
        [JsonPropertyName("currentValue")]
        public string CurrentValue { get; set; }

        /// <summary>
        /// How the device displays UI information for the equalizer setting. This format is product specific.
        /// </summary>
        [JsonPropertyName("deviceUIInfo")]
        public string DeviceUIInfo { get; set; }

        /// <summary>
        /// Indicates whether the equalizer setting is currently available.
        /// </summary>
        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }

        /// <summary>
        /// The equalizer setting.
        /// </summary>
        [JsonPropertyName("target")]
        public EqualizerTarget Target { get; set; }

        /// <summary>
        /// The display title for the equalizer setting. "" indicates that this setting has no assigned title.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The product-specific identifier that the service uses to identify the equalizer setting.
        /// "" indicates that this setting has no assigned identifier.
        /// </summary>
        [JsonPropertyName("titleTextID")]
        public string TitleTextID { get; set; }

        /// <summary>
        /// The value type of the currentValue property for the equalizer setting.
        /// </summary>
        [JsonPropertyName("type")]
        public EqualizerTargetType Type { get; set; }
    }
}
