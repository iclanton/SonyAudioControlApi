using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        /// <summary>
        /// Get the current playback mode settings. Not all settings are valid for all products.
        /// Use "" for the target to get the valid settings for the current product.
        /// </summary>
        /// <param name="target">
        /// The name of the playback mode setting to get.
        /// </param>
        /// <param name="uri">
        /// If a device supports multiple sources for the setting,
        /// include the URI of the specific source for which to get information. For more
        /// information about the URI structure, see the Device Resource URI page (https://developer.sony.com/develop/audio-control-api/api-references/device-uri).
        /// If this is null or "" is set, it means all sources for the mode.
        /// </param>
        public async Task<PlaybackModeSettingsResult[]> GetPlaybackModeSettingsAsync(
            PlaybackModeSettingsResult.PlaybackTarget target =
                PlaybackModeSettingsResult.PlaybackTarget.AllTargets,
            string uri = null
        )
        {
            return await this.makeRequestAsync<PlaybackModeSettingsResult[]>(
                ApiLib.AvContent,
                ApiVersion.V10,
                "getPlaybackModeSettings",
                new PlaybackModeSettingsProps() { Target = target, Uri = uri }
            );
        }
    }

    internal sealed class PlaybackModeSettingsProps
    {
        [JsonPropertyName("target")]
        public PlaybackModeSettingsResult.PlaybackTarget Target { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }

    public sealed class PlaybackModeSettingsResult
    {
        [JsonEnumConverter]
        public enum PlaybackTarget
        {
            /// <summary>
            /// All playback settings.
            /// </summary>
            [EnumJsonStringValue("")]
            AllTargets,

            /// <summary>
            /// Set whether playback starts automatically.
            /// </summary>
            [EnumJsonStringValue("autoPlayback")]
            AutoPlayback,

            /// <summary>
            /// Playback Mode
            /// </summary>
            [EnumJsonStringValue("playType")]
            PlayType,

            /// <summary>
            /// Repeat type
            /// </summary>
            [EnumJsonStringValue("repeatType")]
            RepeatType,

            /// <summary>
            /// Shuffle type.
            /// </summary>
            [EnumJsonStringValue("shuffleType")]
            ShuffleType
        }

        public sealed class PlaybackCandidate
        {
            /// <summary>
            /// Indicates whether the setting is currently available.
            /// </summary>
            [JsonPropertyName("isAvailable")]
            public bool IsAvailable { get; set; }

            /// <summary>
            /// The maximum value of the setting, or -1 if the value type is non-numeric.
            /// </summary>
            [JsonPropertyName("max")]
            public double Max { get; set; }

            /// <summary>
            /// The minimum value of the setting, or -1 if the value type is non-numeric.
            /// </summary>
            [JsonPropertyName("min")]
            public double Min { get; set; }

            /// <summary>
            /// The step value of the setting, or -1 if the value type is non-numeric.
            /// </summary>
            [JsonPropertyName("step")]
            public double Step { get; set; }

            /// <summary>
            /// The display title for the setting. "" indicates that this setting has no assigned title.
            /// </summary>
            [JsonPropertyName("title")]
            public string Title { get; set; }

            /// <summary>
            /// The product-specific identifier that the service uses to identify the setting.
            /// "" indicates that this setting has no assigned identifier.
            /// </summary>
            [JsonPropertyName("titleTextID")]
            public string TitleTextID { get; set; }

            /// <summary>
            /// The current value of the setting. If this property is "" or omitted, then the current value of the
            /// setting is an integer in a defined range with a fixed step.
            /// </summary>
            [JsonPropertyName("value")]
            public string Value { get; set; }
        }

        /// <summary>
        /// Gets an array that provides additional information about the setting. If the setting
        /// is not available on this product, then this property will be null.
        /// </summary>
        [JsonPropertyName("candidate")]
        public PlaybackCandidate[] Candidates { get; set; }

        /// <summary>
        /// The current value of the setting.
        /// </summary>
        /// <remarks>
        /// In case "target" is <see cref="PlaybackTarget.AutoPlayback"/>
        ///     "on" - Enable auto playback function.
        ///     "off" - Disable auto playback function.
        /// In case "target" is <see cref="PlaybackTarget.PlayType"/>
        ///     "normal" - Normal playback
        ///     "folder" - Playback enabled for a unit of folder and its subfolder
        ///     "repeatAll" - In case current composed of multiple parts, repeat playback enabled for whole parts.
        ///     "repeatFolder" - Repeat playback enabled for a unit of folder and its subfolder.
        ///     "repeatTrack" - Repeat playback enabled for a unit of track (audio content) or title (video content).
        ///     "shuffleAll" - In case current composed of multiple parts, shuffle playback enabled for whole parts.
        /// In case "target" is <see cref="PlaybackTarget.RepeatType"/>
        ///     "all" - In case current composed of multiple parts, repeat playback enabled for whole parts.
        ///     "folder" - Repeat playback enabled for a unit of folder and its subfolder.
        ///     "track" - Repeat playback enabled for a unit of track (audio content) or title (video content).
        ///     "chapter" - Repeat playback enabled for a unit of chapter.
        ///     "off" - Repeat playback disabled as a device setting.
        /// In case "target" is <see cref="PlaybackTarget.ShuffleType"/>
        ///     "folder" - Shuffle of a unit of folder and its subfolder.of file name.
        ///     "off" - Shuffle playback disabled as a device setting.
        /// </remarks>
        [JsonPropertyName("currentValue")]
        public string CurrentValue { get; set; }

        /// <summary>
        /// How the device displays UI information for the setting. This format is product specific.
        /// </summary>
        [JsonPropertyName("deviceUIInfo")]
        public string DeviceUIInfo { get; set; }

        /// <summary>
        /// Indicates whether the setting is currently available.
        /// </summary>
        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; set; }

        /// <summary>
        /// The name of the playback mode setting.
        /// </summary>
        [JsonPropertyName("target")]
        public PlaybackTarget Target { get; set; }

        /// <summary>
        /// The display title for the setting. "" indicates that this setting has no assigned title.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The product-specific identifier that the service uses to identify the setting.
        /// "" indicates that this setting has no assigned identifier.
        /// </summary>
        [JsonPropertyName("titleTextID")]
        public string TitleTextID { get; set; }

        /// <summary>
        /// The value type of the currentValue property for the setting.
        /// </summary>
        [JsonPropertyName("type")]
        public TargetType Type { get; set; }

        /// <summary>
        /// The URI of the specific source to which this setting applies, or "" if the device has only
        /// one source for the setting. For more information about the URI structure, see the
        /// Device Resource URI page (https://developer.sony.com/develop/audio-control-api/api-references/device-uri).
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
