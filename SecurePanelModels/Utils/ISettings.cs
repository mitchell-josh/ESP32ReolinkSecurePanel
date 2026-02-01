namespace SecurePanelModels.Utils;

public interface ISettings
{
    string? ReolinkURL { get; }
    
    string? Username { get; }
    
    string? Password { get; }
    
    string? ConnectionString { get; }
}