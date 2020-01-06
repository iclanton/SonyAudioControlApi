using System;
using System.Collections.Generic;
using System.Text;

namespace SonyAudioControlApi
{
    internal static class Utilities
    {
        public static string GetApiLibName(ApiLib lib)
        {
            switch (lib)
            {
                case ApiLib.System:
                    return "system";

                case ApiLib.AvContent:
                    return "avContent";

                case ApiLib.Audio:
                    return "audio";

                default:
                    throw new ArgumentOutOfRangeException(nameof(lib));
            }
        }
    }
}
