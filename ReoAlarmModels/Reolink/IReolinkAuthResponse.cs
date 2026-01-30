using System.Text.Json.Serialization;

namespace ReoAlarmModels.Reolink;

public interface IReolinkAuthResponse
{
    IReolinkAuthToken? Token { get; set; }
}