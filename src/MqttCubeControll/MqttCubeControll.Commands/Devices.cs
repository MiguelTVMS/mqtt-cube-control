// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using MqttCubeControl.Input;

namespace MqttCubeControll.Commands
{
    public class Devices : List<IDevice>, IActionable
    {
        private IDevice GetBySide(int side) => Find(d => d.Side == side);

        public void Act(Movement movement) => GetBySide(movement.Side).Act(movement);

        public async Task ActAsync(Movement movement) => await GetBySide(movement.Side).ActAsync(movement);
    }
}
