using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Represents the request payload to update buzzer alarm settings on the device.
/// Targets the "SetBuzzerAlarmV20" command.
/// </summary>
public record SetBuzzerAlarmRequest(
    // Gets or sets the parameters containing the buzzer configuration to be updated.
    SetBuzzerAlarmParam? Param) : BaseRequest("SetBuzzerAlarmV20", 0);