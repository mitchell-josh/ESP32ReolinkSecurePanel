namespace SecurePanelAPI.Utils;

/// <summary>
/// Central repository for system-wide constants.
/// Using constants ensures that security policies remain consistent across the app.
/// </summary>
public class Consts
{
    /// <summary>
    /// The name of the custom authorization policy that checks for a valid PIN.
    /// Used in: [Authorize(Policy = Consts.AlarmCodePolicy)]
    /// </summary>
    public const string AlarmCodePolicy = "AlarmCode";
    
    /// <summary>
    /// The name of the authentication scheme. 
    /// Useful if you expand to multiple auth types (e.g., JWT for Web vs AlarmCode for Keypads).
    /// </summary>
    public const string AlarmCodeScheme = "AlarmCodeScheme";
}