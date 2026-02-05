using SecurePanelDb.Models;
using SecurePanelModels.Queries;

namespace SecurePanelAPI.Models;

public interface IAlarmCodeService
{
    string HashAlarmCode(AlarmUser alarmUser, string alarmCode);

    bool CheckAlarmCode(AlarmUser alarmUser, string hashedCode, string providedCode);
    
    Task<AlarmResult<bool>> ChangeAlarmCode(string username, string newAlarmCode);
}