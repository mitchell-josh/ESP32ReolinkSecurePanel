using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

/// <summary>
/// Manages the audible siren/alarm states for the camera hardware.
/// </summary>
public interface IAudioAlarmService
{
    // <summary>
    /// Updates the audio alarm configuration (enable/disable or volume) based 
    /// on the settings associated with the provided query criteria.
    /// </summary>
    /// <param name="query">Criteria to identify which channel or scheme configuration to apply.</param>
    /// <returns>An AlarmResult indicating if the hardware update was successful.</returns>
    Task<AlarmResult<bool>> UpdateAudioAlarm(AlarmSchemeQuery query);
}