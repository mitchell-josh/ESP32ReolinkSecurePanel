using SecurePanelDb.Models;

namespace SecurePanelModels.Services;

public interface IAlarmCodeService
{
    string HashAlarmCode(AlarmUser alarmUser, string alarmCode);

    bool CheckAlarmCode(AlarmUser alarmUser, string hashedCode, string providedCode);
}