// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using System.Text.Json;

namespace MqttCubeControl.App
{
    internal class JsonLowercaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            name = char.ToLowerInvariant(name[1]).ToString() + name.Substring(1);
            return name;
        }
    }
}