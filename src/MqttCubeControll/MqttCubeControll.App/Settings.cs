// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using System.Text.Json.Serialization;

namespace MqttCubeControl.App
{
    public class Settings
    {
        [JsonPropertyName("mqtt")]
        public MqttCubeControl.Mqtt.ConnectionSettings Mqtt { get; set; } = new Mqtt.ConnectionSettings();

        [JsonPropertyName("sides")]
        public List<string> Sides { get; set; } = new();
    }
}
