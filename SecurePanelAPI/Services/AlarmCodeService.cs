using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecurePanelAPI.Models;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.Queries;

namespace SecurePanelAPI.Services;

public class AlarmCodeService(SecurePanelDbContext db) : IAlarmCodeService
{
    private readonly IPasswordHasher<AlarmUser> passwordHasher = new PasswordHasher<AlarmUser>();

    public string HashAlarmCode(AlarmUser alarmUser, string alarmCode)
        => this.passwordHasher.HashPassword(alarmUser, alarmCode);

    public bool CheckAlarmCode(AlarmUser alarmUser, string hashedCode, string providedCode)
    {
        var result = this.passwordHasher.VerifyHashedPassword(alarmUser, hashedCode, providedCode);
        return result == PasswordVerificationResult.Success;
    }

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