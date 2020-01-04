using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        /// <summary>
        /// Gets information about the playing content or current selected input.
        /// If the device is not currently playing content, then the response state parameter is "STOPPED".
        /// </summary>
        /// <param name="output">
        /// The URI of the output. Omit this field or use "" for all outputs for the device.
        /// For more information about the URI structure, see
        /// the Device Resource URI page (https://developer.sony.com/develop/audio-control-api/api-references/device-uri).
        /// </param>
        /// <remarks>
        /// Only extInput:* sources have types implemented.
        /// </remarks>
        public async Task<PlayingContentInfoResult[]> GetPlayingContentInfoAsync(string output = null)
        {
            return await ApiRequest.MakeRequestAsync<PlayingContentInfoResult[]>(
                this.Device,
                ApiLib.AvContent,
                ApiVersion.V12,
                "getPlayingContentInfo",
                new PlayingContentInfoProps() { Output = output ?? "" }
            );
        }
    }

    internal sealed class PlayingContentInfoProps
    {
        [JsonPropertyName("output")]
        public string Output { get; set; }
    }

    public sealed class PlayingContentInfoResult
    {
        public class PlayingContentStateInfo {
            [JsonEnumConverter]
            public enum PlayingContentState
            {
                /// <summary>
                /// Content is being played
                /// </summary>
                [EnumJsonStringValue("PLAYING")]
                Playing,

                /// <summary>
                /// Content is stopped
                /// </summary>
                [EnumJsonStringValue("STOPPED")]
                Stopped,

                /// <summary>
                /// Content is pausing
                /// </summary>
                [EnumJsonStringValue("PAUSED")]
                Paused,

                /// <summary>
                /// Content is being forwarded
                /// </summary>
                [EnumJsonStringValue("FORWARDING")]
                Forwarding
            }

            [JsonEnumConverter]
            public enum PlayingContentSupplementalInfo
            {
                /// <summary>
                /// Interrupting and switching to Emergency Warning System.
                /// </summary>
                [EnumJsonStringValue("alarmInterrupting")]
                AlarmInterrupting,

                /// <summary>
                /// Changing to next content by AMS (Automatic Music Scan) function.
                /// </summary>
                [EnumJsonStringValue("automaticMusicScanning")]
                AutomaticMusicScanning,

                /// <summary>
                /// Presetting broadcast stations automatically.
                /// </summary>
                [EnumJsonStringValue("autoPresetting")]
                AutoPresetting,

                /// <summary>
                /// Scanning for DAB digital radio automatically.
                /// </summary>
                [EnumJsonStringValue("autoScanning")]
                AutoScanning,

                /// <summary>
                /// Backward seeking broadcast stations.
                /// </summary>
                [EnumJsonStringValue("bwdSeeking")]
                BwdSeeking,

                /// <summary>
                /// Enumerating storage device.
                /// </summary>
                [EnumJsonStringValue("enumerating")]
                Enumerating,

                /// <summary>
                /// Forward seeking broadcast stations.
                /// </summary>
                [EnumJsonStringValue("fwdSeeking")]
                FwdSeeking,

                /// <summary>
                /// Initial scanning for DAB digital radio.
                /// </summary>
                [EnumJsonStringValue("initialScanning")]
                InitialScanning,

                /// <summary>
                /// Loading a disc storage device.
                /// </summary>
                [EnumJsonStringValue("loading")]
                Loading,

                /// <summary>
                /// Seeking broadcast stations manually.
                /// </summary>
                [EnumJsonStringValue("manualSeeking")]
                ManualSeeking,

                /// <summary>
                /// There is no content that can be played back.
                /// </summary>
                [EnumJsonStringValue("noContent")]
                NoContent,

                /// <summary>
                /// There is no media.
                /// </summary>
                [EnumJsonStringValue("noMedia")]
                NoMedia,

                /// <summary>
                /// There is no next content in current playback scope.
                /// </summary>
                [EnumJsonStringValue("noNextContent")]
                NoNextContent,

                /// <summary>
                /// There is no previous content in current playback scope.
                /// </summary>
                [EnumJsonStringValue("noPreviousContent")]
                NoPreviousContent,

                /// <summary>
                /// A device can not play back for some reason.
                /// </summary>
                [EnumJsonStringValue("notAvailable")]
                NotAvailable,

                /// <summary>
                /// Memorizing preset of broadcast station.
                /// </summary>
                [EnumJsonStringValue("presetMemorizing")]
                PresetMemorizing,

                /// <summary>
                /// Reading a structure of storage device.
                /// </summary>
                [EnumJsonStringValue("reading")]
                Reading,

                /// <summary>
                /// Receiving DAB digital radio. (Before initial scan of DAB etc.)
                /// </summary>
                [EnumJsonStringValue("receiving")]
                Receiving,

                /// <summary>
                /// This content can not be controlled such as pause, stop, or scan by pausePlayingContent,
                /// stopPlayingContent, setPlaySpeed, or scanPlayingContent, and so on.
                /// </summary>
                [EnumJsonStringValue("uncontrollable")]
                Uncontrollable
            }

            /// <summary>
            /// Playing status.
            /// </summary>
            [JsonPropertyName("state")]
            public PlayingContentState State { get; set; }

            public PlayingContentSupplementalInfo? SupplementalInfo { get; set; }
        }

        [JsonPropertyName("stateInfo")]
        public PlayingContentStateInfo StateInfo { get; set; }

        [JsonPropertyName("contentKind")]
        public string ContentKind { get; set; }

        [JsonPropertyName("output")]
        public string Output { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
