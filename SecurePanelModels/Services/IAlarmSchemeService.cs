using SecurePanelModels.AlarmScheme;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

/// <summary>
/// Defines the core business logic for managing security modes and hardware synchronization.
/// </summary>
public interface IAlarmSchemeService
{
    /// <summary>
    /// Retrieves a specific alarm configuration based on provided filters (Channel, Type, or ID).
    /// </summary>
    Task<AlarmResult<AlarmSchemeDto>> GetAlarmScheme(AlarmSchemeQuery query);
    
    /// <summary>
    /// Persists a configuration profile to the data store.
    /// </summary>
    Task<AlarmResult<bool>> SaveAlarmScheme(AlarmSchemeDto scheme);

    /// <summary>
    /// The "Master Switch": Iterates through all configured channels and applies 
    /// the specific settings for the chosen security mode to the physical hardware.
    /// </summary>
    Task<AlarmResult<bool>> SetAlarm(AlarmSchemeTypes alarmSchemeType);
}