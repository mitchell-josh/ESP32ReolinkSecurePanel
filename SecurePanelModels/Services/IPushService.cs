namespace SecurePanelModels.Services;

public interface IPushService
{
    Task<bool> UpdatePush(int channelId);
}