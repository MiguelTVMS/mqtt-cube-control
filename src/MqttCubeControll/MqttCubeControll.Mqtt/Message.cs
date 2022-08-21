// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using System.Text.Json;

namespace MqttCubeControl.Mqtt
{
    public class Message<T>
    {
        private string _clientID;
        private string _topic;
        private T? _payload;

        public string ClientID { get => _clientID; }
        public string Topic { get => _topic; }

        public T? Payload { get => _payload; }

        private Message(string clientID, string topic, T? payload)
        {
            _clientID = clientID;
            _topic = topic;
            _payload = payload;
        }

        public static Message<Tf> Create<Tf>(string topic, Tf payload, string clientID = "MqttCubeControl")
        {
            return new Message<Tf>(clientID, topic, payload);
        }

        public static Message<Tf> Create<Tf>(string topic, string payload, string clientID = "MqttCubeControl")
        {
            Tf? serializedPayload = default(Tf);
            if (!string.IsNullOrEmpty(payload))
                serializedPayload = JsonSerializer.Deserialize<Tf>(payload);

            return new Message<Tf>(clientID, topic, serializedPayload);
        }

    }

}
