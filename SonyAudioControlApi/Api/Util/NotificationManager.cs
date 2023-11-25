using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Websocket.Client;

namespace SonyAudioControlApi
{
    public sealed partial class Api
    {
        public delegate void NotificationEventHandler<TResult>(
            DeviceDescriptor sender,
            TResult result
        );

        public event NotificationEventHandler<CurrentExternalTerminalsStatusResult[]> OnExternalTerminalStatusNotification;
        public event NotificationEventHandler<PlayingContentInfoResult> OnPlayingContentInfoNotification;
        public event NotificationEventHandler<PowerStatusResult[]> OnPowerStatusNotification;
        public event NotificationEventHandler<VolumeInformationResult[]> OnVolumeInformationNotification;

        private class NotificationManager
        {
            private ApiLibNotificationManager audioNotificationManager;
            private ApiLibNotificationManager avContentNotificationManager;
            private ApiLibNotificationManager systemNotificationManager;

            private Dictionary<
                ApiLib,
                Dictionary<ApiVersion, Dictionary<string, EventInfo>>
            > handlers;

            public DeviceDescriptor Device { get; private set; }

            public Api Api { get; set; }

            public NotificationManager(DeviceDescriptor device, Api api)
            {
                this.Device = device;
                this.Api = api;

                this.initializeSubscriptions();

                this.audioNotificationManager = new ApiLibNotificationManager(
                    this.Device,
                    ApiLib.Audio,
                    this.getSubscriptionsForLib(ApiLib.Audio)
                );
                this.avContentNotificationManager = new ApiLibNotificationManager(
                    this.Device,
                    ApiLib.AvContent,
                    this.getSubscriptionsForLib(ApiLib.AvContent)
                );
                this.systemNotificationManager = new ApiLibNotificationManager(
                    this.Device,
                    ApiLib.System,
                    this.getSubscriptionsForLib(ApiLib.System)
                );

                this.audioNotificationManager.OnNotification += this.OnNotification;
                this.avContentNotificationManager.OnNotification += this.OnNotification;
                this.systemNotificationManager.OnNotification += this.OnNotification;
            }

            private void OnNotification(
                ApiLibNotificationManager sender,
                string method,
                ApiVersion version,
                string serializedParams
            )
            {
                Dictionary<ApiVersion, Dictionary<string, EventInfo>> libDelegates = this.handlers[
                    sender.Lib
                ];
                if (libDelegates != null)
                {
                    Dictionary<string, EventInfo> versionDelegates = libDelegates[version];
                    if (versionDelegates != null)
                    {
                        EventInfo eventInfo = versionDelegates[method];
                        if (eventInfo != null)
                        {
                            // We expect this to be the argument in NotificationEventHandler<>
                            Type genericArgumentType = eventInfo
                                .EventHandlerType
                                .GenericTypeArguments[0];
                            dynamic notificationResult = JsonSerializer.Deserialize(
                                serializedParams,
                                genericArgumentType
                            );
                            FieldInfo eventFieldInfo = eventInfo
                                .DeclaringType
                                .GetField(
                                    eventInfo.Name,
                                    BindingFlags.Instance | BindingFlags.NonPublic
                                );
                            dynamic eventField = eventFieldInfo.GetValue(this.Api);
                            try
                            {
                                eventField.Invoke(this.Device, notificationResult);
                            }
                            catch
                            {
                                // Ignore handler errors
                            }
                        }
                    }
                }
            }

            public void Initialize()
            {
                Task.WaitAll(
                    this.systemNotificationManager.InitializeAsync(),
                    this.audioNotificationManager.InitializeAsync(),
                    this.avContentNotificationManager.InitializeAsync()
                );
            }

            private void initializeSubscriptions()
            {
                Type apiType = this.Api.GetType();

                this.handlers = new Dictionary<
                    ApiLib,
                    Dictionary<ApiVersion, Dictionary<string, EventInfo>>
                >()
                {
                    [ApiLib.Audio] = new Dictionary<ApiVersion, Dictionary<string, EventInfo>>()
                    {
                        [ApiVersion.V10] = new Dictionary<string, EventInfo>()
                        {
                            ["notifyVolumeInformation"] = apiType.GetEvent(
                                nameof(this.Api.OnVolumeInformationNotification)
                            )
                        }
                    },
                    [ApiLib.AvContent] = new Dictionary<ApiVersion, Dictionary<string, EventInfo>>()
                    {
                        [ApiVersion.V10] = new Dictionary<string, EventInfo>()
                        {
                            ["notifyPlayingContentInfo"] = apiType.GetEvent(
                                nameof(this.Api.OnPlayingContentInfoNotification)
                            ),
                            ["notifyExternalTerminalStatus"] = apiType.GetEvent(
                                nameof(this.Api.OnExternalTerminalStatusNotification)
                            )
                        }
                    },
                    [ApiLib.System] = new Dictionary<ApiVersion, Dictionary<string, EventInfo>>()
                    {
                        [ApiVersion.V10] = new Dictionary<string, EventInfo>()
                        {
                            ["notifyPowerStatus"] = apiType.GetEvent(
                                nameof(this.Api.OnPowerStatusNotification)
                            )
                        }
                    }
                };
            }

            private IEnumerable<ApiLibNotificationManager.NotificationSubscription> getSubscriptionsForLib(
                ApiLib lib
            )
            {
                Dictionary<ApiVersion, Dictionary<string, EventInfo>> libDelegates = this.handlers[
                    lib
                ];
                if (libDelegates != null)
                {
                    return libDelegates.SelectMany(
                        (versionElement) =>
                            versionElement
                                .Value
                                .Keys
                                .Select(
                                    (methodElement) =>
                                        new ApiLibNotificationManager.NotificationSubscription()
                                        {
                                            Name = methodElement,
                                            Version = versionElement.Key
                                        }
                                )
                    );
                }
                else
                {
                    return new ApiLibNotificationManager.NotificationSubscription[0];
                }
            }
        }

        private class ApiLibNotificationManager : IDisposable
        {
            public class NotificationSubscription
            {
                public static bool operator ==(
                    NotificationSubscription obj1,
                    NotificationSubscription obj2
                )
                {
                    return obj1.Equals(obj2);
                }

                public static bool operator !=(
                    NotificationSubscription obj1,
                    NotificationSubscription obj2
                )
                {
                    return !obj1.Equals(obj2);
                }

                [JsonPropertyName("name")]
                public string Name { get; set; }

                [JsonPropertyName("version")]
                public ApiVersion Version { get; set; }

                public override bool Equals(object obj)
                {
                    return obj is NotificationSubscription subscription
                        && this.Name == subscription.Name
                        && this.Version == subscription.Version;
                }

                public override int GetHashCode()
                {
                    return HashCode.Combine(Name, Version);
                }
            }

            private class NotificationParms
            {
                [JsonPropertyName("enabled")]
                public NotificationSubscription[] Enabled { get; set; }

                [JsonPropertyName("disabled")]
                public NotificationSubscription[] Disabled { get; set; }
            }

            private class NotificationRequest : RequestObject
            {
                public NotificationRequest(
                    NotificationSubscription[] enabled = null,
                    NotificationSubscription[] disabled = null
                )
                    : base(
                        "switchNotifications",
                        ApiVersion.V10,
                        new NotificationParms()
                        {
                            Enabled = enabled ?? new NotificationSubscription[0],
                            Disabled = disabled ?? new NotificationSubscription[0]
                        }
                    ) { }
            }

            private class NotificationObject
            {
                [JsonPropertyName("method")]
                public string Method { get; set; }

                [JsonPropertyName("params")]
                public object[] Params { get; set; }

                [JsonPropertyName("version")]
                public ApiVersion Version { get; set; }
            }

            private WebsocketClient websocket;

            private HashSet<NotificationSubscription> subscriptions;

            public delegate void InitializedEventHandler(ApiLibNotificationManager sender);

            public delegate void MessageEventHandler(
                ApiLibNotificationManager sender,
                string method,
                ApiVersion version,
                string serializedParams
            );

            public DeviceDescriptor Device { get; private set; }

            public ApiLib Lib { get; private set; }

            public bool IsInitialized { get; private set; }

            public event InitializedEventHandler OnInitialized;
            public event MessageEventHandler OnNotification;

            public async Task InitializeAsync()
            {
                await this.initializeWebsocketAsync();
            }

            public ApiLibNotificationManager(
                DeviceDescriptor device,
                ApiLib lib,
                IEnumerable<NotificationSubscription> subscriptions
            )
            {
                this.Device = device;
                this.Lib = lib;
                this.subscriptions = new HashSet<NotificationSubscription>(subscriptions);
            }

            private async Task initializeWebsocketAsync()
            {
                // If we happen to have one laying around, clean it up first
                if (this.websocket != null)
                {
                    try
                    {
                        this.websocket.Dispose();
                    }
                    catch
                    {
                        // Ignore
                    }
                }

                this.websocket = new WebsocketClient(
                    new Uri(
                        $"ws://{this.Device.Hostname}:{this.Device.Port}/sony/{Utilities.GetApiLibName(this.Lib)}"
                    )
                );
                this.websocket.ReconnectTimeout = TimeSpan.FromSeconds(30);

                // Send an empty switchNotifications to get the available notification types
                NotificationRequest subscriptionsRequest = null;

                Action initialSubscribe = () =>
                {
                    subscriptionsRequest = new NotificationRequest(
                        enabled: new NotificationSubscription[0],
                        disabled: new NotificationSubscription[0]
                    );

                    this.websocket.Send(subscriptionsRequest.Serialized);
                };

                this.websocket
                    .ReconnectionHappened
                    .Subscribe(
                        (message) =>
                        {
                            Debug.WriteLine(
                                $"NOTIFY ({this.Lib}): subscription reconnected ({message})"
                            );
                            //initialSubscribe();
                        }
                    );

                this.websocket
                    .MessageReceived
                    .Subscribe(
                        async (message) =>
                        {
                            await Task.Run(() =>
                            {
                                Debug.WriteLine(
                                    $"NOTIFY ({this.Lib}): message received ({message})"
                                );

                                SlimResponseObject basicResponse =
                                    JsonSerializer.Deserialize<SlimResponseObject>(message.Text);
                                if (basicResponse.Id == subscriptionsRequest?.Id)
                                {
                                    // This is the response to a notification subscription
                                    ResponseObject<NotificationParms> response =
                                        JsonSerializer.Deserialize<
                                            ResponseObject<NotificationParms>
                                        >(message.Text);

                                    // Let's find out if any subscriptions are missing
                                    bool anySubscriptionsMissing = this.subscriptions.Any(
                                        (subscription) =>
                                        {
                                            bool subscribed = response
                                                .Result
                                                .Enabled
                                                .Contains(subscription);
                                            bool available =
                                                !subscribed
                                                && response.Result.Disabled.Contains(subscription);
                                            return !subscribed && available;
                                        }
                                    );
                                    if (anySubscriptionsMissing)
                                    {
                                        HashSet<NotificationSubscription> availableSubscriptions =
                                            new HashSet<NotificationSubscription>();
                                        availableSubscriptions.UnionWith(response.Result.Disabled);
                                        availableSubscriptions.UnionWith(response.Result.Enabled);

                                        List<NotificationSubscription> enabled =
                                            new List<NotificationSubscription>();
                                        List<NotificationSubscription> disabled =
                                            new List<NotificationSubscription>();
                                        foreach (
                                            NotificationSubscription available in availableSubscriptions
                                        )
                                        {
                                            if (this.subscriptions.Contains(available))
                                            {
                                                enabled.Add(available);
                                            }
                                            else
                                            {
                                                disabled.Add(available);
                                            }
                                        }

                                        // Subscribe to everything - we don't expect too much traffic
                                        // This may need to change later
                                        subscriptionsRequest = new NotificationRequest(
                                            enabled: enabled.ToArray(),
                                            disabled: disabled.ToArray()
                                        );

                                        this.websocket.Send(subscriptionsRequest.Serialized);
                                    }
                                    else
                                    {
                                        bool newInitialization = !this.IsInitialized;
                                        this.IsInitialized = true;
                                        if (newInitialization)
                                        {
                                            try
                                            {
                                                this.OnInitialized?.Invoke(this);
                                            }
                                            catch
                                            {
                                                // Swallow handler errors
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // This is a notification
                                    NotificationObject notification =
                                        JsonSerializer.Deserialize<NotificationObject>(
                                            message.Text
                                        );
                                    try
                                    {
                                        this.OnNotification?.Invoke(
                                            this,
                                            notification.Method,
                                            notification.Version,
                                            JsonSerializer.Serialize(notification.Params)
                                        );
                                    }
                                    catch
                                    {
                                        // Swallow handler errors
                                    }
                                }
                            });
                        }
                    );

                await this.websocket.Start();
                initialSubscribe();
            }

            public void Dispose()
            {
                if (this.websocket != null)
                {
                    this.websocket.Dispose();
                }
            }
        }
    }
}
