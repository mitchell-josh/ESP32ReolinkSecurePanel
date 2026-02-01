using Microsoft.AspNetCore.Identity;
using SecurePanelDb.Models;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

public class AlarmCodeService : IAlarmCodeService
{
    private readonly IPasswordHasher<AlarmUser> passwordHasher = new PasswordHasher<AlarmUser>();

    public string HashAlarmCode(AlarmUser alarmUser, string alarmCode)
        => this.passwordHasher.HashPassword(alarmUser, alarmCode);

    public bool CheckAlarmCode(AlarmUser alarmUser, string hashedCode, string providedCode)
    {
        var result = this.passwordHasher.VerifyHashedPassword(alarmUser, hashedCode, providedCode);
        return result == PasswordVerificationResult.Success;
    }
}