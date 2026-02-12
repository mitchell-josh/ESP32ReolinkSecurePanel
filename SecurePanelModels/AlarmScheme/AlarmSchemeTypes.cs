namespace SecurePanelModels.AlarmScheme;

/// <summary>
/// Defines the high-level security modes for the system.
/// This acts as a strategy selector for camera behavior.
/// Must be kept in sync with UI code.
/// </summary>
public enum AlarmSchemeTypes
{
    /// <summary>
    /// System is inactive. Typically mutes sirens and push notifications 
    /// while owners are home.
    /// </summary>
    Disarmed,
    
    /// <summary>
    /// "Stay" mode. Usually enables perimeter push notifications 
    /// but keeps internal sirens or certain camera zones silent.
    /// </summary>
    PartialAlarm,
    
    /// <summary>
    /// "Away" mode. Maximum security: all AI detections trigger 
    /// sirens, recording, and immediate push alerts.
    /// </summary>
    FullAlarm
}