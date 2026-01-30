namespace ReoAlarmModels.Reolink;

public interface IReolinkUser : IReolinkParam
{
    string Version { get; }
    
    string Username { get; }
    
    string Password { get; }
}