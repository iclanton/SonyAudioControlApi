using SonyAudioControlApi.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        /// <summary>
        /// Gets the current power status of the device.
        /// </summary>
        /// <returns></returns>
        public async Task<GetPowerStatusResult> GetPowerStatusAsync()
        {
            return await ApiRequest.MakeRequestAsync<GetPowerStatusResult>(
                this.Device,
                ApiLib.System,
                ApiVersion.V11,
                "getPowerStatus"
            );
        }
    }

    [DataContract]
    public sealed class GetPowerStatusResult
    {
        public enum PowerStatus
        {
            /// <summary>
            /// The device is transitioning to the power-on state.
            /// </summary>
            [EnumStringValue("activating")]
            Activating,

            /// <summary>
            /// The device is in the power-on state.
            /// </summary>
            [EnumStringValue("active")]
            Active,

            /// <summary>
            /// The device is transitioning to the power-off state.
            /// </summary>
            [EnumStringValue("shuttingDown")]
            ShuttingDown,

            /// <summary>
            /// The device is in the standby state. Network functions are active, and the device can switch to the
            /// power-on state via a network command. Not all products support standby, personalaudio products don't.
            /// </summary>
            [EnumStringValue("standby")]
            Standby
        }

        public enum PowerStandbyDetail
        {
            /// <summary>
            /// The device is in its normal standby state.
            /// </summary>
            [EnumStringValue("normalStandby")]
            NormalStandby,

            /// <summary>
            /// The device is in its quick-start standby state. The device can transition quickly to an active state.
            /// </summary>
            [EnumStringValue("quickStartStandby")]
            QuickStartStandby
        }

        /// <summary>
        /// The current power status of the device
        /// </summary>
        [DataMember(Name = "status")]
        public PowerStatus Status { get; set; }

        /// <summary>
        /// Additional information for the standby power state.
        /// If this value is null, then no additional information is available.
        /// </summary>
        [DataMember(Name = "standbyDetail")]
        public PowerStandbyDetail? StandbyDetail { get; set; }
    }
}
