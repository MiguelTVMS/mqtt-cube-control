// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

namespace MqttCubeControl.Input
{
    public class Movement
    {
        private int _side;
        private Action _action;
        private RotateDirection _rotateDirection;
        private decimal _rotateAngle;

        public int Side { get => _side; }
        public Action Action { get => _action; }
        public RotateDirection RotateDirection { get => _rotateDirection; }
        public decimal RotateAngle { get => _rotateAngle; }

        private Movement()
        {
            _side = -1;
            _action = Action.None;
            _rotateDirection = RotateDirection.None;
            _rotateAngle = Decimal.Zero;
        }

        internal Movement(int side, Action action, RotateDirection rotateDirection, decimal rotateAngle)
        {
            _side = side;
            _action = action;
            _rotateDirection = rotateDirection;
            _rotateAngle = (_rotateDirection != RotateDirection.None) ? Math.Abs(rotateAngle) : decimal.Zero;
        }

        public override string ToString()
        {
            var message = $"Movement from action \"{_action.ToString().ToLowerInvariant()}\"";

            if (_rotateDirection != RotateDirection.None)
                message += $" in the {_rotateDirection.ToString().ToLowerInvariant()} in a {_rotateAngle}° angle";


            message += ".";

            return message;
        }
    }
}
