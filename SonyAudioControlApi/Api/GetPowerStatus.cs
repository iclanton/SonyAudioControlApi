using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        /// <summary>
        /// Gets the current power status of the device.
        /// </summary>
        public async Task<PowerStatusResult> GetPowerStatusAsync()
        {
            return await ApiRequest.MakeRequestAsync<PowerStatusResult>(
                this.Device,
                ApiLib.System,
                ApiVersion.V11,
                "getPowerStatus"
            );
        }
    }

    public sealed class PowerStatusResult
    {
        [JsonEnumConverter]
        public enum PowerStatus
        {
            /// <summary>
            /// The device is transitioning to the power-on state.
            /// </summary>
            [EnumJsonStringValue("activating")]
            Activating,

            /// <summary>
            /// The device is in the power-on state.
            /// </summary>
            [EnumJsonStringValue("active")]
            Active,

            /// <summary>
            /// The device is transitioning to the power-off state.
            /// </summary>
            [EnumJsonStringValue("shuttingDown")]
            ShuttingDown,

            /// <summary>
            /// The device is in the standby state. Network functions are active, and the device can switch to the
            /// power-on state via a network command. Not all products support standby, personalaudio products don't.
            /// </summary>
            [EnumJsonStringValue("standby")]
            Standby
        }

        [JsonEnumConverter]
        public enum PowerStandbyDetail
        {
            /// <summary>
            /// The device is in its normal standby state.
            /// </summary>
            [EnumJsonStringValue("normalStandby")]
            NormalStandby,

            /// <summary>
            /// The device is in its quick-start standby state. The device can transition quickly to an active state.
            /// </summary>
            [EnumJsonStringValue("quickStartStandby")]
            QuickStartStandby
        }

        /// <summary>
        /// The current power status of the device
        /// </summary>
        [JsonPropertyName("status")]
        public PowerStatus Status { get; set; }

        /// <summary>
        /// Additional information for the standby power state.
        /// If this value is null, then no additional information is available.
        /// </summary>
        [JsonPropertyName("standbyDetail")]
        public PowerStandbyDetail? StandbyDetail { get; set; }
    }
}
