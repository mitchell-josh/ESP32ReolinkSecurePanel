using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

/// <summary>
/// Represents the request payload to retrieve buzzer alarm settings.
/// Specifically targets the "GetBuzzerAlarmV20" API command.
/// </summary>
public record GetBuzzerAlarmRequest(
    /* Gets or sets the target parameters for the query.
     Defaults to a new instance to allow for immediate property assignment. */
    BuzzerAlarmParam Param) : BaseRequest("GetBuzzerAlarmV20", 0);