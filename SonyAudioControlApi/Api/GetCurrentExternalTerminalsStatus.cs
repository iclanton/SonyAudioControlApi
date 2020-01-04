using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        /// <summary>
        /// Gets information about the current status of all external input and output terminal sources of
        /// the device. For a device that has no external input or output connectors, this APi should
        /// return an empty result with no error codes.
        /// </summary>
        public async Task<CurrentExternalTerminalsStatusResult[]> GetCurrentExternalTerminalsStatusAsync()
        {
            return await ApiRequest.MakeRequestAsync<CurrentExternalTerminalsStatusResult[]>(
                this.Device,
                ApiLib.AvContent,
                ApiVersion.V10,
                "getCurrentExternalTerminalsStatus"
            );
        }
    }

    public sealed class CurrentExternalTerminalsStatusResult
    {
        [JsonEnumConverter]
        public enum ActiveStatus
        {
            /// <summary>
            /// The active status could not be determined.
            /// </summary>
            [EnumJsonStringValue("")]
            Unknown,

            /// <summary>
            /// The terminal is enabled or a selected input source.
            /// </summary>
            [EnumJsonStringValue("active")]
            Active,

            /// <summary>
            /// The terminal is disabled or not a selected input source.
            /// </summary>
            [EnumJsonStringValue("inactive")]
            Inactive
        }

        [JsonEnumConverter]
        public enum ConnectionStatus
        {
            /// <summary>
            /// The terminal is connected.
            /// </summary>
            [EnumJsonStringValue("connected")]
            Connected,

            /// <summary>
            /// The terminal is not connected.
            /// </summary>
            [EnumJsonStringValue("unconnected")]
            Unconnected,

            /// <summary>
            /// The connection status is unknown.
            /// </summary>
            [EnumJsonStringValue("unknown")]
            Unknown
        }

        [JsonEnumConverter]
        public enum TerminalMeta
        {
            /// <summary>
            /// An audio system type CEC device is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:audiosystem")]
            Audiosystem,

            /// <summary>
            /// An AV amplifier is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:avamp")]
            Avamp,

            /// <summary>
            /// BD/DVD input
            /// </summary>
            [EnumJsonStringValue("meta:bd-dvd")]
            BdDvd,

            /// <summary>
            /// Bluetooth audio input
            /// </summary>
            [EnumJsonStringValue("meta:btaudio")]
            Btaudio,

            /// <summary>
            /// BT phone input
            /// </summary>
            [EnumJsonStringValue("meta:btphone")]
            Btphone,

            /// <summary>
            /// A video camera is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:camcoder")]
            Camcoder,

            /// <summary>
            /// Coaxial digital audio input
            /// </summary>
            [EnumJsonStringValue("meta:coaxial")]
            Coaxial,

            /// <summary>
            /// A complex device is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:complex")]
            Complex,

            /// <summary>
            /// Component input (Y and Pb/Cb and Pr/Cr connectors)
            /// </summary>
            [EnumJsonStringValue("meta:component")]
            Component,

            /// <summary>
            /// D-Component input
            /// </summary>
            [EnumJsonStringValue("meta:componentd")]
            Componentd,

            /// <summary>
            /// Composite input
            /// </summary>
            [EnumJsonStringValue("meta:composite")]
            Composite,

            /// <summary>
            /// Composite and D-Component combined input
            /// </summary>
            [EnumJsonStringValue("meta:composite_componentd")]
            Composite_componentd,

            /// <summary>
            /// A digital camera is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:digitalcamera")]
            Digitalcamera,

            /// <summary>
            /// A disk player is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:disc")]
            Disc,

            /// <summary>
            /// D-subminiature 15pin input
            /// </summary>
            [EnumJsonStringValue("meta:dsub15")]
            Dsub15,

            /// <summary>
            /// A game console is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:game")]
            Game,

            /// <summary>
            /// HDMI input
            /// </summary>
            [EnumJsonStringValue("meta:hdmi")]
            Hdmi,

            /// <summary>
            /// HDMI output
            /// </summary>
            [EnumJsonStringValue("meta:hdmi:output")]
            HdmiOutput,

            /// <summary>
            /// A home theater device is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:hometheater")]
            Hometheater,

            /// <summary>
            /// Axillary input
            /// </summary>
            [EnumJsonStringValue("meta:line")]
            Line,

            /// <summary>
            /// A mini audio port, the exact hardware port is device dependent
            /// </summary>
            [EnumJsonStringValue("meta:linemini")]
            Linemini,

            /// <summary>
            /// Optical digital audio input
            /// </summary>
            [EnumJsonStringValue("meta:optical")]
            Optical,

            /// <summary>
            /// A personal computer is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:pc")]
            Pc,

            /// <summary>
            /// A playback type CEC device is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:playbackdevice")]
            Playbackdevice,

            /// <summary>
            /// A recording type CEC device is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:recordingdevice")]
            Recordingdevice,

            /// <summary>
            /// SCART input
            /// </summary>
            [EnumJsonStringValue("meta:scart")]
            Scart,

            /// <summary>
            /// S-Video input
            /// </summary>
            [EnumJsonStringValue("meta:svideo")]
            Svideo,

            /// <summary>
            /// A tape player is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:tape")]
            Tape,

            /// <summary>
            /// A tuner is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:tuner")]
            Tuner,

            /// <summary>
            /// A tuner type CEC device is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:tunerdevice")]
            Tunerdevice,

            /// <summary>
            /// A TV type CEC device is connected to the terminal
            /// </summary>
            [EnumJsonStringValue("meta:tv")]
            Tv,

            /// <summary>
            /// USB DAC input
            /// </summary>
            [EnumJsonStringValue("meta:usbdac")]
            Usbdac,

            /// <summary>
            /// WiFi Display input
            /// </summary>
            [EnumJsonStringValue("meta:wifidisplay")]
            Wifidisplay,

            /// <summary>
            /// Wireless transceiver
            /// </summary>
            [EnumJsonStringValue("meta:wirelessTransceiver:output")]
            WirelessTransceiverOutput,

            /// <summary>
            /// Source input
            /// </summary>
            [EnumJsonStringValue("meta:source")]
            Source,

            /// <summary>
            /// SACD/CD input
            /// </summary>
            [EnumJsonStringValue("meta:sacd-cd")]
            SacdCd,

            /// <summary>
            /// SAT/CATV input
            /// </summary>
            [EnumJsonStringValue("meta:sat-catv")]
            SatCatv,

            /// <summary>
            /// Video input
            /// </summary>
            [EnumJsonStringValue("meta:video")]
            Video,

            /// <summary>
            /// Zone output
            /// </summary>
            [EnumJsonStringValue("meta:zone:output")]
            ZoneOutput
        }

        /// <summary>
        /// The active status of the terminal. For a terminal type of "meta:zone:output", the active status indicates
        /// whether the zone is enabled. For all other terminal types, the active status indicates whether the source
        /// is selected as an input source for any output zone.
        /// </summary>
        [JsonPropertyName("active")]
        public ActiveStatus Active { get; set; }

        /// <summary>
        /// The connection status of the terminal.
        /// </summary>
        [JsonPropertyName("connection")]
        public ConnectionStatus Connection { get; set; }

        /// <summary>
        /// The icon URL that the service uses for the terminal, or "" if the service does not define an icon.
        /// </summary>
        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; }

        /// <summary>
        /// The label that the user assigned to this terminal.
        /// </summary>
        [JsonPropertyName("label")]
        public string Label { get; set; }

        /// <summary>
        /// Describes the type of terminal. For example, this can provide a hint to an application as to which
        /// icon to show to the user. The type is provided using a "meta" URI format. Your application should
        /// customize its UI based on the type of the terminal, such as choosing an appropriate image.
        /// </summary>
        [JsonPropertyName("meta")]
        public TerminalMeta Meta { get; set; }

        /// <summary>
        /// An array of the URIs of the output terminals that are available for this input terminal.
        /// For more information about the URI structure, see the
        /// Device Resource URI page (https://developer.sony.com/develop/audio-control-api/api-references/device-uri).
        /// For an output terminal, this parameter is omitted or its value is null.
        /// </summary>
        [JsonPropertyName("outputs")]
        public string[] Outputs { get; set; }

        /// <summary>
        /// The name of the input or output terminal.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The URI of the external terminal. For more information about the URI structure, see the
        /// Device Resource URI page (https://developer.sony.com/develop/audio-control-api/api-references/device-uri).
        /// </summary>
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
