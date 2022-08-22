// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using Microsoft.Extensions.Configuration;
using MqttCubeControl.Helpers;
using MqttCubeControl.Input.Sources;
using MqttCubeControl.Mqtt;
using MqttCubeControll.Commands;
using MqttCubeControll.Commands.Windows;

namespace MqttCubeControl.App // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static Settings _settings = new Settings();
        static IConnection? _connection;
        static readonly CancellationTokenSource _cancellationSource = new();

        private static Devices _devices = new();

        static async Task Main(string[] args)
        {
            LoadSettings();
            _settings.DumpToConsole();
            _connection = new MqttConnection(_settings.Mqtt);
            await _connection.Connect(_cancellationSource.Token);
            await _connection.Subscribe<Zigbee2mqtt>(ProcessEvent, _cancellationSource.Token);
            Console.ReadLine();
        }

        static async Task ProcessEvent<T>(Message<T> data) where T : ISource
        {
            var source = data.Payload;
            if (source is null || !source.HasAction())
            {
                await Task.CompletedTask;
                return;
            }

            await _devices.ActAsync(source.ToMovement());

        }
        private static void LoadSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build();

            _settings = config.GetRequiredSection("settings").Get<Settings>();

            _devices.Clear();
            for (int i = 0; i < _settings.Sides.Count; i++)
            {
                switch (_settings.Sides[i].ToLowerInvariant())
                {
                    case "volume":
                        _devices.Add(new Volume(i));
                        break;
                    default:
                        throw new Exception($"There's no contol implementation for \"{_settings.Sides[i]}\".");
                }
            }

        }
    }
}