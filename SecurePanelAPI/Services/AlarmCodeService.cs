using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecurePanelAPI.Models;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.Queries;

namespace SecurePanelAPI.Services;

/// <summary>
/// Concrete implementation of the alarm credential management logic.
/// Utilizes the ASP.NET Core Identity PasswordHasher for cryptographically secure PIN storage.
/// </summary>
public class AlarmCodeService(SecurePanelDbContext db) : IAlarmCodeService
{
    // The PasswordHasher is thread-safe and provides salt generation and hashing out of the box.
    private readonly IPasswordHasher<AlarmUser> passwordHasher = new PasswordHasher<AlarmUser>();

    /// <summary>
    /// Generates a salted hash for a new PIN.
    /// </summary>
    public string HashAlarmCode(AlarmUser alarmUser, string alarmCode)
        => this.passwordHasher.HashPassword(alarmUser, alarmCode);

    /// <summary>
    /// Low-level verification of a provided PIN against a stored hash.
    /// </summary>
    public bool CheckAlarmCode(AlarmUser alarmUser, string hashedCode, string providedCode)
    {
        var result = this.passwordHasher.VerifyHashedPassword(alarmUser, hashedCode, providedCode);
        return result == PasswordVerificationResult.Success;
    }

    /// <summary>
    /// High-level orchestration for user verification. 
    /// Bridges the Gap between the API request and the Database entities.
    /// </summary>
    public async Task<AlarmResult<bool>> CheckAlarmCode(string username, string alarmCode)
    {
        var alarmUser = await db.AlarmUsers.SingleOrDefaultAsync(u => u.Username == username);

        if (alarmUser == null)
        {
            return AlarmResult<bool>.Failure("User not found");
        }

        var result = this.CheckAlarmCode(alarmUser, alarmUser.AlarmCodeHash!, alarmCode);
        
        return result ? AlarmResult<bool>.Success(result) : AlarmResult<bool>.Failure("Invalid alarm code.");
    }

    /// <summary>
    /// Securely updates a user's alarm code and persists it to the database.
    /// </summary>
    public async Task<AlarmResult<bool>> ChangeAlarmCode(string username, string newAlarmCode)
    {
        var alarmUser = await db.AlarmUsers.SingleOrDefaultAsync(u => u.Username == username);

        if (alarmUser == null)
        {
            return AlarmResult<bool>.Failure("User not found");
        }
        else
        {
            var newHashedAlarmCode = this.passwordHasher.HashPassword(alarmUser, newAlarmCode);
            alarmUser.AlarmCodeHash = newHashedAlarmCode;
            await db.SaveChangesAsync();
            return AlarmResult<bool>.Success(true);
        }
    }
}