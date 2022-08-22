// Copyright (c) 2022 João Miguel <joao@miguel.ms>
// This program is distributed under the terms of the GNU General Public License

using System.Text.Json.Serialization;

namespace MqttCubeControl.Input.Sources
{
    /// <summary>
    /// Support to Xiaomi Aqara Smart Home Cube "MFKZQ01LM" connected to zigbee2mqtt.
    /// Exposes battery, voltage, action_angle, device_temperature, power_outage_count, action_from_side, action_side, action_to_side, side, action, linkquality
    /// </summary>
    /// <see href="https://www.zigbee2mqtt.io/devices/MFKZQ01LM.html">Zigbee2MQTT Xiaomi Aqara Smart Home Cube Page</see>
    public class Zigbee2mqtt : ISource
    {
        /// <summary>
        /// Remaining battery in %.
        /// Value can be found in the published state on the battery property.
        /// The minimal value is 0 and the maximum value is 100.
        /// The unit of this value is %.
        /// </summary>
        [JsonPropertyName("battery")]
        public int? Battery { get; set; }

        /// <summary>
        /// Voltage of the battery in millivolts.
        /// Value can be found in the published state on the voltage property.
        /// The unit of this value is mV.
        /// </summary>
        [JsonPropertyName("current")]
        public int? Current { get; set; }

        /// <summary>
        /// Temperature of the device.
        /// Value can be found in the published state on the device_temperature property.
        /// The unit of this value is °C.
        /// </summary>
        [JsonPropertyName("device_temperature")]
        public int? DeviceTemperature { get; set; }

        /// <summary>
        /// Link quality (signal strength).
        /// Value can be found in the published state on the linkquality property.
        /// The minimal value is 0 and the maximum value is 255.
        /// The unit of this value is lqi.
        /// </summary>
        [JsonPropertyName("linkquality")]
        public int? Linkquality { get; set; }

        [JsonPropertyName("power")]
        public int? Power { get; set; }

        /// <summary>
        /// Number of power outages.
        /// Value can be found in the published state on the power_outage_count property.
        /// </summary>
        [JsonPropertyName("power_outage_count")]
        public int? PowerOutageCount { get; set; }

        /// <summary>
        /// Side of the cube.
        /// Value can be found in the published state on the side property.
        /// The minimal value is 0 and the maximum value is 6.
        /// </summary>
        [JsonPropertyName("side")]
        public int? Side { get; set; }

        /// <summary>
        /// Voltage of the battery in millivolts.
        /// Value can be found in the published state on the voltage property.
        /// The unit of this value is mV.
        /// </summary>
        [JsonPropertyName("voltage")]
        public int? Voltage { get; set; }

        /// <summary>
        /// Triggered action (e.g. a button click).
        /// Value can be found in the published state on the action property. 
        /// The possible values are: shake, wakeup, fall, tap, slide, flip180, flip90, rotate_left, rotate_right.
        /// </summary>
        [JsonPropertyName("action")]
        public string? Action { get; set; }

        /// <summary>
        /// Value can be found in the published state on the action_angle property.
        /// The minimal value is -360 and the maximum value is 360.
        /// The unit of this value is °.
        /// </summary>
        [JsonPropertyName("action_angle")]
        public decimal? ActionAngle { get; set; }

        /// <summary>
        /// Side of the cube.
        /// Value can be found in the published state on the action_from_side property. 
        /// The minimal value is 0 and the maximum value is 6.
        /// </summary>
        [JsonPropertyName("action_from_side")]
        public int? ActionFromSide { get; set; }

        /// <summary>
        /// Side of the cube.
        /// Value can be found in the published state on the action_side property. 
        /// The minimal value is 0 and the maximum value is 6.
        /// </summary>
        [JsonPropertyName("action_side")]
        public int? ActionSide { get; set; }

        /// <summary>
        /// Side of the cube. 
        /// Value can be found in the published state on the action_to_side property. 
        /// The minimal value is 0 and the maximum value is 6.
        /// </summary>
        [JsonPropertyName("action_to_side")]
        public int? ActionToSide { get; set; }

        public bool HasAction() => (!string.IsNullOrEmpty(Action));

        public Movement ToMovement() => new Movement(ConvertSide(), ConvertAction(), ConvertRotateDirection(), ActionAngle ?? Decimal.Zero);

        private int ConvertSide() => Side.HasValue ? Side.Value : -1;

        private Action ConvertAction()
        {
            if (string.IsNullOrEmpty(Action))
                return Input.Action.None;

            switch (Action.ToLowerInvariant())
            {
                case "rotate_left":
                case "rotate_right":
                    return Input.Action.Rotate;
                case "shake":
                    return Input.Action.Shake;
                case "wakeup":
                    return Input.Action.Wakeup;
                case "fall":
                    return Input.Action.Fall;
                case "tap":
                    return Input.Action.Tap;
                case "slide":
                    return Input.Action.Slide;
                case "flip180":
                    return Input.Action.Flip180;
                case "flip90":
                    return Input.Action.Flip90;
                default:
                    throw new ArgumentOutOfRangeException($"The action value \"{Action}\" is not supported.", nameof(Action));
            }
        }
        private RotateDirection ConvertRotateDirection()
        {
            if (string.IsNullOrEmpty(Action))
                return Input.RotateDirection.None;

            switch (Action.ToLowerInvariant())
            {
                case "rotate_left":
                    return Input.RotateDirection.Left;
                case "rotate_right":
                    return Input.RotateDirection.Right;
                default:
                    return Input.RotateDirection.None;
            }

        }
    }
}
