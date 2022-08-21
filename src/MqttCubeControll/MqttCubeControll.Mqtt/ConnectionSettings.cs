// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using System.Text.Json.Serialization;

namespace MqttCubeControl.Mqtt
{
    public class ConnectionSettings
    {
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; } = "MqttCubeControl";

        [JsonPropertyName("protocol")]
        public string Protocol { get; set; } = "mqtt";

        [JsonPropertyName("host")]
        public string? Host { get; set; } = "localhost";

        [JsonPropertyName("basePath")]
        public string? BasePath { get; set; } = string.Empty;

        [JsonPropertyName("port")]
        public int Port { get; set; } = 1883;

        [JsonPropertyName("tls")]
        public bool Tls { get; set; } = false;

        [JsonPropertyName("validateTls")]
        public bool ValidateTls { get; set; } = true;

        [JsonPropertyName("useraname")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string? Password { get; set; } = string.Empty;

        [JsonPropertyName("topic")]
        public string? Topic { get; set; } = string.Empty;

        public string Uri { get => $"{Protocol}://{Host}:{Port}/{BasePath}"; }

        public bool isAutenticated { get => !string.IsNullOrEmpty(Username); }

        public bool isSecure { get => (Tls); }

    }

}

