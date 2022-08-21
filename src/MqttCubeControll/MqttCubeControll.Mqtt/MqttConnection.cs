// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using MQTTnet;
using MQTTnet.Client;

namespace MqttCubeControl.Mqtt
{
    public class MqttConnection : IConnection
    {
        ConnectionSettings? _settings;
        MqttFactory _mqttFactory = new MqttFactory();
        MqttClientOptions? _clientOptions;
        IMqttClient? _mqttClient;

        public MqttConnection(ConnectionSettings settings)
        {
            _settings = settings;
            Initialize();
        }

        private void Initialize()
        {
            if (_settings is null)
                throw new ApplicationException("Settings not found.");

            var _clientOptionsBuilder = new MqttClientOptionsBuilder().WithClientId(_settings.ClientId);

            switch (_settings.Protocol.ToLowerInvariant())
            {
                case "ws":
                    _clientOptionsBuilder = _clientOptionsBuilder.WithWebSocketServer(_settings.Uri);
                    break;
                case "mqtt":
                    _clientOptionsBuilder = _clientOptionsBuilder.WithTcpServer(_settings.Host, _settings.Port);
                    break;
                default:
                    throw new IndexOutOfRangeException("Connection should be \"mqtt\" or \"ws\".");
            }

            if (_settings.isAutenticated)
                _clientOptionsBuilder = _clientOptionsBuilder.WithCredentials(_settings.Username, _settings.Password);

            if (_settings.isSecure)
                _clientOptionsBuilder = _clientOptionsBuilder.WithTls(new MqttClientOptionsBuilderTlsParameters()
                {
                    UseTls = _settings.Tls,
                    AllowUntrustedCertificates = !_settings.ValidateTls
                });

            _clientOptions = _clientOptionsBuilder.Build();
        }

        public async Task Subscribe<T>(Func<Message<T>, Task> task, CancellationToken cancellationToken = default)
        {
            if (_settings is null)
                throw new ApplicationException("Settings not found.");

            await Subscribe<T>(task, _settings.Topic, cancellationToken);
        }

        public async Task Subscribe<T>(Func<Message<T>, Task> task, string? topic, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(topic)) throw new ArgumentNullException("Topic cannot be null or empty.", nameof(topic));


            if (_mqttClient is null || !_mqttClient.IsConnected)
                throw new Exception("The MQTT client is not connected.");

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var message = Message<T>.Create<T>(e.ApplicationMessage.Topic, e.ApplicationMessage.ConvertPayloadToString(), e.ClientId);
                await task(message);
            };

            var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic(topic); })
                .Build();


            await _mqttClient.SubscribeAsync(mqttSubscribeOptions, cancellationToken);
            Console.WriteLine($"MQTT client subscribed to topic {topic}.");

        }

        private Task _mqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            throw new NotImplementedException();
        }

        public async Task Connect(CancellationToken cancellationToken = default)
        {
            _mqttClient = _mqttFactory.CreateMqttClient();

            // This will throw an exception if the server is not available.
            // The result from this message returns additional data which was sent 
            // from the server. Please refer to the MQTT protocol specification for details.
            await _mqttClient.ConnectAsync(_clientOptions, cancellationToken);
            Console.WriteLine("The MQTT client is connected.");

        }

        public async Task Disconnect(CancellationToken cancellationToken = default)
        {
            if (_mqttClient is null || !_mqttClient.IsConnected)
                throw new Exception("The MQTT client is not connected.");

            // Send a clean disconnect to the server by calling _DisconnectAsync_. Without this the TCP connection
            // gets dropped and the server will handle this as a non clean disconnect (see MQTT spec for details).
            var mqttClientDisconnectOptions = _mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
            await _mqttClient.DisconnectAsync(mqttClientDisconnectOptions, cancellationToken);
            Console.WriteLine("The MQTT client is disconnected.");
        }
    }
}
