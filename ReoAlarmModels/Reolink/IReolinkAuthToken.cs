namespace ReoAlarmModels.Reolink;

public interface IReolinkAuthToken
{
    double? LeaseTime { get; }
    
    string? Name { get; }
}