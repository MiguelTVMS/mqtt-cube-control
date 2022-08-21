﻿// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

namespace MqttCubeControl.Input.Sources
{
    public interface ISource
    {
        public bool HasAction();

        public Movement ToMovement();
    }
}