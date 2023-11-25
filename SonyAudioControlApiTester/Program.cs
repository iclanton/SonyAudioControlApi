using System;
using System.Threading.Tasks;
using SonyAudioControlApi;

namespace SonyAudioControlApiTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Api api = new Api(
                new DeviceDescriptor()
                {
                    Hostname = args[0],
                    Type = DeviceDescriptor.DeviceType.SoundbarReceiver
                }
            );
            var result1 = await api.GetPowerStatusAsync();
            var result2 = await api.GetCurrentExternalTerminalsStatusAsync();
            var result3 = await api.GetCustomEqualizerSettingsAsync();
            var result4 = await api.GetInterfaceInformationAsync();
            var result5 = await api.GetPlaybackModeSettingsAsync();
            var result6 = await api.GetPlayingContentInfoAsync();
            var result7 = await api.GetVolumeInformationAsync();

            api.InitializeNotifications();

            api.OnVolumeInformationNotification += (
                DeviceDescriptor sender,
                VolumeInformationResult[] result
            ) =>
            {
                foreach (VolumeInformationResult volume in result)
                {
                    Console.WriteLine($"Volume ({volume.Output}): {volume.Volume}");
                }
            };

            Console.ReadLine();
        }
    }
}
