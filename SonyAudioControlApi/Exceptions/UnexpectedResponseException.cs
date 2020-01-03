using System;
using System.Collections.Generic;
using System.Text;

namespace SonyAudioControlApi.Exceptions
{
    public class UnexpectedResponseException : Exception
    {
        internal UnexpectedResponseException(string property, string actualValue) :
            base($"Recieved unexpected value \"{actualValue}\" for property \"{property}\".")
        {
        }
    }
}
