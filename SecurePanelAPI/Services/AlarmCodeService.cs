using Microsoft.AspNetCore.Identity;
using SecurePanelAPI.Models;
using SecurePanelDb;
using SecurePanelDb.Models;

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

    public bool ChangeAlarmCode(string username, string newAlarmCode)
    {
        var alarmUser = db.AlarmUsers.SingleOrDefault(u => u.Username == username);

        if (alarmUser == null)
        {
            return false;
        }
        else
        {
            var newHashedAlarmCode = this.passwordHasher.HashPassword(alarmUser, newAlarmCode);
            alarmUser.AlarmCodeHash = newHashedAlarmCode;
            db.SaveChanges();
            return true;
        }
    }
}