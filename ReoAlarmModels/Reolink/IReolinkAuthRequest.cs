namespace ReoAlarmModels.Reolink;

public interface IReolinkAuthRequest : IReolinkParam
{
    string? Version { get; }
    
    string? Username { get; }
    
    string? Password { get; }
}