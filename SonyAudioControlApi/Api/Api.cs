using System;
using System.Collections.Generic;
using System.Text;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        private NotificationManager notificationsManager { get; set; }

        public DeviceDescriptor Device { get; private set; }

        public Api(DeviceDescriptor device)
        {
            this.Device = device;
            this.notificationsManager = new NotificationManager(this.Device, this);
        }

        public void InitializeNotifications()
        {
            this.notificationsManager.Initialize();
        }
    }
}
