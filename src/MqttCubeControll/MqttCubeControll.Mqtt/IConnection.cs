// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

namespace MqttCubeControl.Mqtt
{
    public interface IConnection
    {
        Task Connect(CancellationToken cancellationToken = default);
        Task Disconnect(CancellationToken cancellationToken = default);
        Task Subscribe<T>(Func<Message<T>, Task> task, string topic, CancellationToken cancellationToken = default);
        Task Subscribe<T>(Func<Message<T>, Task> task, CancellationToken cancellationToken = default);
    }
}
