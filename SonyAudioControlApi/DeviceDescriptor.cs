using System;
using System.Collections.Generic;
using System.Text;

namespace SonyAudioControlApi
{
    public class DeviceDescriptor
    {
        public enum DeviceType
        {
            SoundbarReceiver,
            WirelessSpeaker
        }

        public string Hostname { get; set; }

        public DeviceType Type { get; set; }
    }
}
