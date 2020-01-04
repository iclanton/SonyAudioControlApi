using System;
using System.Threading.Tasks;
using SonyAudioControlApi;

namespace SonyAudioControlApiTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Api api = new Api(new DeviceDescriptor() { Hostname = args[0], Type = DeviceDescriptor.DeviceType.SoundbarReceiver });
            var result1 = await api.GetPowerStatusAsync();
            var result2 = await api.GetCurrentExternalTerminalsStatusAsync();
            var result3 = await api.GetCustomEqualizerSettingsAsync();
        }
    }
}
