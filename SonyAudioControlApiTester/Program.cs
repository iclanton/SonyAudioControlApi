using System;
using System.Threading.Tasks;
using SonyAudioControlApi;

namespace SonyAudioControlApiTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Api api = new Api(new DeviceDescriptor() { Hostname = "[IP_ADDR]", Type = DeviceDescriptor.DeviceType.SoundbarReceiver });
            var powerStatus = await api.GetPowerStatusAsync();
        }
    }
}
