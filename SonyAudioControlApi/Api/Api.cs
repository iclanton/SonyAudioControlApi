﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SonyAudioControlApi
{
    public partial class Api
    {
        public DeviceDescriptor Device { get; private set; }

        public Api(DeviceDescriptor device)
        {
            this.Device = device;
        }
    }
}