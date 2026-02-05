using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

public interface IPushService
{
    Task<AlarmResult<bool>> UpdatePush(AlarmSchemeQuery query);
}