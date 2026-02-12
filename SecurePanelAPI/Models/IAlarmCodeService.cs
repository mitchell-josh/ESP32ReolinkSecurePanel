using SecurePanelDb.Models;
using SecurePanelModels.Queries;

namespace SecurePanelAPI.Models;

/// <summary>
/// Manages the hashing, verification, and lifecycle of user security codes.
/// </summary>
public interface IAlarmCodeService
{
    /// <summary>
    /// Converts a plain-text PIN into a secure, salted hash.
    /// Should be used when creating users or updating codes.
    /// </summary>
    string HashAlarmCode(AlarmUser alarmUser, string alarmCode);

    /// <summary>
    /// Performs a constant-time comparison between a stored hash and a provided PIN.
    /// </summary>
    bool CheckAlarmCode(AlarmUser alarmUser, string hashedCode, string providedCode);

    /// <summary>
    /// Orchestrates a database lookup and PIN verification for a specific username.
    /// Typically used by the AuthController for login/verification checks.
    /// </summary>
    Task<AlarmResult<bool>> CheckAlarmCode(string username, string alarmCode);
    
    /// <summary>
    /// Updates a user's security code in the database after hashing it.
    /// </summary>
    Task<AlarmResult<bool>> ChangeAlarmCode(string username, string newAlarmCode);
}