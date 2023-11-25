using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        /// <summary>
        /// Gets the current volume level and mute status.
        /// </summary>
        /// <param name="output">
        /// The URI of the output. Omit this field or use "" to affect all outputs for the device.
        /// For more information about the URI structure, see
        /// the Device Resource URI page (https://developer.sony.com/develop/audio-control-api/api-references/device-uri).
        /// </param>
        public async Task<VolumeInformationResult[]> GetVolumeInformationAsync(string output = null)
        {
            return await this.makeRequestAsync<VolumeInformationResult[]>(
                ApiLib.Audio,
                ApiVersion.V11,
                "getVolumeInformation",
                new VolumeInformationProps() { Output = output ?? "" }
            );
        }
    }

    internal sealed class VolumeInformationProps
    {
        [JsonPropertyName("output")]
        public string Output { get; set; }
    }

    public sealed class VolumeInformationResult
    {
        [JsonEnumConverter]
        public enum MuteStatus
        {
            /// <summary>
            /// The device does not support mute.
            /// </summary>
            [EnumJsonStringValue("")]
            NotSupported,

            /// <summary>
            /// Not muted.
            /// </summary>
            [EnumJsonStringValue("off")]
            Off,

            /// <summary>
            /// Muted.
            /// </summary>
            [EnumJsonStringValue("on")]
            On,

            /// <summary>
            ///  Unknown; the device can only toggle the mute setting.
            /// </summary>
            [EnumJsonStringValue("toggle")]
            Toggle
        }

        /// <summary>
        /// The maximum volume level of the output; or -1 if no maximum value is available or
        /// if the device does not support setting the volume by an absolute value.
        /// </summary>
        [JsonPropertyName("maxVolume")]
        public int MaxVolume { get; set; }

        /// <summary>
        /// The minimum volume level of the output; or -1 if no minimum value is available or
        /// if the device does not support setting the volume by an absolute value.
        /// </summary>
        [JsonPropertyName("minVolume")]
        public int MinVolume { get; set; }

        /// <summary>
        /// The current mute status of the output.
        /// </summary>
        [JsonPropertyName("mute")]
        public MuteStatus Mute { get; set; }

        /// <summary>
        /// The URI of the output. For more information about the URI structure, see
        /// the Device Resource URI page (https://developer.sony.com/develop/audio-control-api/api-references/device-uri).
        /// "" refers to all outputs of the device.
        /// </summary>
        [JsonPropertyName("output")]
        public string Output { get; set; }

        /// <summary>
        /// The volume level step value for the output; or 0 if the device only supports setting the volume by an absolute value.
        /// </summary>
        [JsonPropertyName("step")]
        public int Step { get; set; }

        /// <summary>
        /// The current volume level of the output; or -1 if no volume information is available or
        /// if the device does not support setting the volume by an absolute value.
        /// </summary>
        [JsonPropertyName("volume")]
        public int Volume { get; set; }
    }
}
