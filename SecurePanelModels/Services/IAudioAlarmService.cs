using SecurePanelModels.DTOs;

namespace SecurePanelModels.Services;

public interface IAudioAlarmService
{
    Task<bool> UpdateAudioAlarm(AlarmSettingsDto channel);
}