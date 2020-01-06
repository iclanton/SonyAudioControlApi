using System;
using System.Collections.Generic;
using System.Text;

namespace SonyAudioControlApi
{
    public class DeviceDescriptor
    {
        private const int RECEIVER_SOUNDBAR_PORT = 10000;
        private const int WIRELESS_SPEAKER_PORT = 54480;

        public enum DeviceType
        {
            SoundbarReceiver,
            WirelessSpeaker
        }

        public string Hostname { get; set; }

        public DeviceType Type { get; set; }

        internal int Port
        {
            get
            {
                switch (this.Type)
                {
                    case DeviceDescriptor.DeviceType.SoundbarReceiver:
                        return RECEIVER_SOUNDBAR_PORT;

                    case DeviceDescriptor.DeviceType.WirelessSpeaker:
                        return WIRELESS_SPEAKER_PORT;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(this.Type));
                }
            }
        }
    }
}
