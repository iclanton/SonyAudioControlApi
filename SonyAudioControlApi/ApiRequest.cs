using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    internal static class ApiRequest
    {
        private const int RECEIVER_SOUNDBAR_PORT = 10000;
        private const int WIRELESS_SPEAKER_PORT = 54480;

        private class RequestObject
        {
            private static int idCounter = 1;

            [JsonPropertyName("id")]
            public int Id { get; private set; }

            [JsonPropertyName("method")]
            public string Method { get; private set; }

            [JsonPropertyName("params")]
            public object[] Params { get; private set; }

            [JsonPropertyName("version")]
            public string Version { get; private set; }

            public RequestObject(string method, ApiVersion version, object @params)
            {
                if (string.IsNullOrEmpty(method))
                {
                    throw new ArgumentNullException(nameof(method));
                }

                this.Id = RequestObject.idCounter++;

                if (RequestObject.idCounter > 0x7FFFFFFF)
                {
                    RequestObject.idCounter = 1;
                }

                this.Method = method;

                if (@params is null)
                {
                    this.Params = new object[] { };
                }
                else
                {
                    this.Params = new object[] { @params };
                }

                switch (version)
                {
                    case ApiVersion.V10:
                        this.Version = "1.0";
                        break;
                    case ApiVersion.V11:
                        this.Version = "1.1";
                        break;
                    case ApiVersion.V12:
                        this.Version = "1.2";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(version));
                }
            }
        }

        private class ResponseObject<TResponse>
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("error")]
            public object Error { get; set; }

            [JsonPropertyName("result")]
            public TResponse[] Result { get; set; }
        }

        public static async Task<TResult> MakeRequestAsync<TResult>(DeviceDescriptor device, ApiLib lib, ApiVersion version, string method, object @params = null)
        {
            if (device is null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            RequestObject requestObject = new RequestObject(method, version, @params);

            int port;
            switch (device.Type)
            {
                case DeviceDescriptor.DeviceType.SoundbarReceiver:
                    port = RECEIVER_SOUNDBAR_PORT;
                    break;
                case DeviceDescriptor.DeviceType.WirelessSpeaker:
                    port = WIRELESS_SPEAKER_PORT;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(device.Type));
            }

            string libName;
            switch (lib)
            {
                case ApiLib.System:
                    libName = "system";
                    break;

                case ApiLib.AvContent:
                    libName = "avContent";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(lib));
            }

            string requestUrl = $"http://{device.Hostname}:{port}/sony/{libName}";

            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(requestObject), Encoding.UTF8, "application/json");
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsync(requestUrl, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    ResponseObject<TResult> responseObject = JsonSerializer.Deserialize<ResponseObject<TResult>>(await response.Content.ReadAsStringAsync());
                    if (responseObject.Error != null)
                    {
                        // TODO - handle
                        throw new Exception("Unexpected error");
                    }
                    else
                    {
                        return responseObject.Result[0];
                    }
                }
                else
                {
                    // TODO - handle this better
                    throw new Exceptions.HttpException();
                }
            }
        }
    }
}
