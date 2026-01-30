using ReoAlarmModels.Reolink;

namespace Reolink.Auth;

public class ReolinkAuthResponse : IReolinkAuthResponse
{
    public IReolinkAuthToken? Token { get; set; }
}