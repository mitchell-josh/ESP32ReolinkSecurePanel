using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

public interface IBuzzerAlarmService
{
    Task<AlarmResult<bool>> UpdateBuzzerAlarm(AlarmSchemeQuery query);
}