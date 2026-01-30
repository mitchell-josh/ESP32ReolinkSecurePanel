using System.Text.Json.Serialization;
using ReoAlarmModels.Reolink;

namespace Reolink.Auth;

public class ReolinkPacket(string? command, int? action, IReolinkParam? param) : IReolinkPacket
{
    [JsonPropertyName("command")]
    public string? Command { get; } = command;

    [JsonPropertyName("action")]
    public int? Action { get; } = action;

    [JsonPropertyName("param")]
    public IReolinkParam? Param { get; } = param;
    
    public void Send(string rootUrl)
    {
    }
}