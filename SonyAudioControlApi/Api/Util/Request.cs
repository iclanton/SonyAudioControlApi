using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
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
            public ApiVersion Version { get; private set; }

            public RequestObject() { }

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

                this.Version = version;
            }

            [JsonIgnore]
            public string Serialized
            {
                get { return JsonSerializer.Serialize(this); }
            }
        }

        private class SlimResponseObject
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
        }

        private class ResponseObject<TResponse> : SlimResponseObject
        {
            [JsonPropertyName("error")]
            public object Error { get; set; }

            [JsonPropertyName("result")]
            [SingleElementArrayConverter]
            public TResponse Result { get; set; }
        }

        private async Task<TResult> makeRequestAsync<TResult>(
            ApiLib lib,
            ApiVersion version,
            string method,
            object @params = null
        )
        {
            if (this.Device is null)
            {
                throw new ArgumentNullException(nameof(this.Device));
            }

            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            RequestObject requestObject = new RequestObject(method, version, @params);

            string libName = Utilities.GetApiLibName(lib);
            string requestUrl = $"http://{this.Device.Hostname}:{this.Device.Port}/sony/{libName}";

            HttpContent httpContent = new StringContent(
                requestObject.Serialized,
                Encoding.UTF8,
                "application/json"
            );
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.PostAsync(requestUrl, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    ResponseObject<TResult> responseObject = JsonSerializer.Deserialize<
                        ResponseObject<TResult>
                    >(await response.Content.ReadAsStringAsync());
                    if (responseObject.Error != null)
                    {
                        // TODO - handle
                        throw new Exception("Unexpected error");
                    }
                    else
                    {
                        return responseObject.Result;
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
