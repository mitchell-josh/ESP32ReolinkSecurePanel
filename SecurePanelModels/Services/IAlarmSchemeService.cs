using SecurePanelModels.AlarmScheme;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

public interface IAlarmSchemeService
{
    Task<AlarmResult<AlarmSchemeDto>> GetAlarmScheme(AlarmSchemeQuery query);
    
    Task<AlarmResult<bool>> SaveAlarmScheme(AlarmSchemeDto scheme);
    
    Task<AlarmResult<List<AlarmSchemeTypeDto>>> GetAlarmSchemeTypes();

    Task<AlarmResult<bool>> SetAlarm(AlarmSchemeTypes alarmSchemeType);
}