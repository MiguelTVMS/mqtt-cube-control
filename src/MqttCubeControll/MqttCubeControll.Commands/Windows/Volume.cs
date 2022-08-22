// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using MqttCubeControl.Input;
using System.Runtime.InteropServices;

namespace MqttCubeControll.Commands.Windows
{
    public class Volume : IDevice, ISide
    {

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        private const int KEYEVENTF_EXTENTEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const int VK_VOLUME_MUTE = 0xAD;
        private const int VK_VOLUME_UP = 0xAF;
        private const int VK_VOLUME_DOWN = 0xAE;

        int _side;

        public Volume(int side)
        {
            _side = side;
        }

        public int Side { get => _side; }

        public void Act(Movement movement)
        {
            switch (movement.RotateDirection)
            {
                case RotateDirection.Right:
                    VolumeUp();
                    break;
                case RotateDirection.Left:
                    VolumeDown();
                    break;
                case RotateDirection.None:
                default:
                    break;
            }

        }

        public async Task ActAsync(Movement movement)
        {
            Act(movement);
            await Task.CompletedTask;
        }

        private void VolumeUp() => keybd_event(VK_VOLUME_UP, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);

        private static void VolumeDown() => keybd_event(VK_VOLUME_DOWN, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);

        private static void Mute() => keybd_event(VK_VOLUME_MUTE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
    }
}
