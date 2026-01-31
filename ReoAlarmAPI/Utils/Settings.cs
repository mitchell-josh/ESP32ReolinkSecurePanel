using ReoAlarmModels.Utils;

namespace ReoAlarmAPI.Utils;

public class Settings(IConfiguration configuration) : ISettings
{
    public string? ReolinkURL => configuration.GetValue<string>("Settings:ReolinkURL");

    public string? Username =>  configuration.GetValue<string>("Settings:Username");
    
    public string? Password => configuration.GetValue<string>("Settings:Password");
}