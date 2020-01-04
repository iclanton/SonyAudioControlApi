using SonyAudioControlApi.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public partial class Api
    {
        public async Task<GetPowerStatusResult> GetPowerStatusAsync()
        {
            PowerStatusApiResponse apiResponse = await ApiRequest.MakeRequestAsync<PowerStatusApiResponse>(this.Device, ApiLib.System, ApiVersion.V11, "getPowerStatus");
            return new GetPowerStatusResult(apiResponse);
        }
    }

    public class GetPowerStatusResult
    {
        public enum PowerStatus
        {
            /// <summary>
            /// The device is transitioning to the power-on state.
            /// </summary>
            Activating,

            /// <summary>
            /// The device is in the power-on state.
            /// </summary>
            Active,

            /// <summary>
            /// The device is transitioning to the power-off state.
            /// </summary>
            ShuttingDown,

            /// <summary>
            /// The device is in the standby state. Network functions are active, and the device can switch to the
            /// power-on state via a network command. Not all products support standby, personalaudio products don't.
            /// </summary>
            Standby
        }

        public enum PowerStandbyDetail
        {
            /// <summary>
            /// The device is in its normal standby state.
            /// </summary>
            NormalStandby,

            /// <summary>
            /// The device is in its quick-start standby state. The device can transition quickly to an active state.
            /// </summary>
            QuickStartStandby
        }

        /// <summary>
        /// The current power status of the device
        /// </summary>
        public PowerStatus Status { get; private set; }

        /// <summary>
        /// Additional information for the standby power state.
        /// If this value is null, then no additional information is available.
        /// </summary>
        public PowerStandbyDetail? StandbyDetail { get; private set; }

        internal GetPowerStatusResult(PowerStatusApiResponse apiResponse)
        {
            switch (apiResponse.Status)
            {
                case "activating":
                    this.Status = PowerStatus.Activating;
                    break;

                case "active":
                    this.Status = PowerStatus.Active;
                    break;

                case "shuttingDown":
                    this.Status = PowerStatus.ShuttingDown;
                    break;

                case "standby":
                    this.Status = PowerStatus.Standby;
                    break;

                default:
                    throw new UnexpectedResponseException(nameof(apiResponse.Status), apiResponse.Status);
            }

            switch (apiResponse.StandbyDetail)
            {
                case null:
                case "":
                    this.StandbyDetail = null;
                    break;

                case "normalStandby":
                    this.StandbyDetail = PowerStandbyDetail.NormalStandby;
                    break;

                case "quickStartStandby":
                    this.StandbyDetail = PowerStandbyDetail.QuickStartStandby;
                    break;

                default:
                    throw new UnexpectedResponseException(nameof(apiResponse.StandbyDetail), apiResponse.StandbyDetail);
            }
        }
    }

    [DataContract]
    internal class PowerStatusApiResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "standbyDetail")]
        public string StandbyDetail { get; set; }
    }
}
