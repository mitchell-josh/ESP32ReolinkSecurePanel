namespace ReoAlarmModels.Reolink;

public interface IReolinkPacket
{
    string? Command { get; }
    
    int? Action { get; }
    
    IReolinkParam? Param { get; }

    void Send(string rootUrl);
}