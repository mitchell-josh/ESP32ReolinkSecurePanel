using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

public interface IAudioAlarmService
{
    Task<AlarmResult<bool>> UpdateAudioAlarm(AlarmSchemeQuery query);
}