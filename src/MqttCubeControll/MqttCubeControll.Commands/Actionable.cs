// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using MqttCubeControl.Input;

namespace MqttCubeControll.Commands
{
    public interface IActionable
    {
        void Act(Movement movement);

        Task ActAsync(Movement movement);
    }
}
