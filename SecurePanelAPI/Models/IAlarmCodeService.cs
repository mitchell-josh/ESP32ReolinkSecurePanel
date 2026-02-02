using SecurePanelDb.Models;

namespace SecurePanelAPI.Models;

public interface IAlarmCodeService
{
    string HashAlarmCode(AlarmUser alarmUser, string alarmCode);

    bool CheckAlarmCode(AlarmUser alarmUser, string hashedCode, string providedCode);
    
    bool ChangeAlarmCode(string username, string newAlarmCode);
}