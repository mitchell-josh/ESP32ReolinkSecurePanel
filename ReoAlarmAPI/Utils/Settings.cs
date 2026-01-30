using ReoAlarmModels.Utils;

namespace ReoAlarmAPI.Utils;

public class Settings(IConfiguration configuration) : ISettings
{
    public string? ReolinkURL { get; } = configuration.GetValue<string>("Settings:ReolinkURL");
}