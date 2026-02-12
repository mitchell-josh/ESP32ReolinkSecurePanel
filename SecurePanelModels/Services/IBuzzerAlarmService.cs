using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

/// <summary>
/// Manages the internal beeper/buzzer states for the Reolink hardware.
/// </summary>
public interface IBuzzerAlarmService
{
    /// <summary>
    /// Updates the buzzer configuration based on the settings 
    /// associated with the provided query criteria.
    /// </summary>
    /// <param name="query">Criteria used to resolve the correct channel and configuration state.</param>
    /// <returns>An AlarmResult indicating success or failure of the hardware update.</returns>
    Task<AlarmResult<bool>> UpdateBuzzerAlarm(AlarmSchemeQuery query);
}