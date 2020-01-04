using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        /// <summary>
        /// This API provides information of WebAPI interface provided by the server.
        /// This API must not include private information.
        /// </summary>
        public async Task<InterfaceInformationResult> GetInterfaceInformationAsync()
        {
            return await ApiRequest.MakeRequestAsync<InterfaceInformationResult>(
                this.Device,
                ApiLib.System,
                ApiVersion.V10,
                "getInterfaceInformation"
            );
        }
    }

    public sealed class InterfaceInformationResult
    {
        [JsonEnumConverter]
        public enum DeviceCategory
        {
            /// <summary>
            /// Cameras and Camcorders.
            /// </summary>
            [EnumJsonStringValue("camera")]
            Camera,

            /// <summary>
            /// TV.
            /// </summary>
            [EnumJsonStringValue("tv")]
            Tv,

            /// <summary>
            /// Internet player with Google TV.
            /// </summary>
            [EnumJsonStringValue("internetTV")]
            InternetTV,

            /// <summary>
            /// The device that can serve downloadable video contents.
            /// </summary>
            [EnumJsonStringValue("videoServer")]
            VideoServer,

            /// <summary>
            /// Home theater system.
            /// </summary>
            [EnumJsonStringValue("homeTheaterSystem")]
            HomeTheaterSystem,

            /// <summary>
            /// Video Player.
            /// </summary>
            [EnumJsonStringValue("videoPlayer")]
            VideoPlayer,

            /// <summary>
            /// Personal Audio product.
            /// </summary>
            [EnumJsonStringValue("personalAudio")]
            PersonalAudio
        }

        /// <summary>
        /// Version for client to change its behavior WRT significant difference within productCategory.
        /// This version is managed/controlled within each productCategory. This parameter is composed of
        /// "[X].[Y].[Z]", where [X], [Y] and [Z] are string representing integer and concatenated with period "." in between.
        /// </summary>
        [JsonPropertyName("interfaceVersion")]
        public string InterfaceVersion { get; set; }

        /// <summary>
        /// Model name.
        /// </summary>
        [JsonPropertyName("modelName")]
        public string ModelName { get; set; }

        /// <summary>
        /// Device category.
        /// </summary>
        [JsonPropertyName("productCategory")]
        public DeviceCategory ProductCategory { get; set; }

        /// <summary>
        /// More detail product information can be returned if productCategory is not enough.
        /// </summary>
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        /// <summary>
        /// Server name. In case device can launch multiple Scalar WebAPI servers, return this server's name for client to distinguish.
        /// </summary>
        [JsonPropertyName("serverName")]
        public string ServerName { get; set; }

    }
}

