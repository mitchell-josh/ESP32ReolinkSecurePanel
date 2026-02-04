using SecurePanelModels.DTOs;

namespace SecurePanelModels.Services;

public interface IAlarmSchemeService
{
    Task<AlarmSchemeDto> GetAlarmScheme(AlarmSchemeDto schemeDto);

    Task SaveAlarmScheme(AlarmSchemeDto scheme);
    
    Task<List<AlarmSchemeTypeDto>> GetAlarmSchemeTypes();
}