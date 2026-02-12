using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

/// <summary>
/// Manages the configuration and state of remote push notifications 
/// for AI detection events.
/// </summary>
public interface IPushService
{
    /// <summary>
    /// Synchronizes the hardware push settings (Enabled/Disabled and AI Schedules) 
    /// based on the configuration resolved by the provided query.
    /// </summary>
    /// <param name="query">Criteria used to identify the target channel and the desired alarm scheme.</param>
    /// <returns>An AlarmResult indicating if the cloud/hardware update was successful.</returns>
    Task<AlarmResult<bool>> UpdatePush(AlarmSchemeQuery query);
}