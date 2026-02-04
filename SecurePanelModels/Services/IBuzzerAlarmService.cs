using SecurePanelModels.DTOs;

namespace SecurePanelModels.Services;

public interface IBuzzerAlarmService
{
    Task<bool> UpdateBuzzerAlarm(AlarmSettingsDto channel);
}