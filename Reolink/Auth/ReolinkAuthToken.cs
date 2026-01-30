using ReoAlarmModels.Reolink;

namespace Reolink.Auth;

public class ReolinkAuthToken(double? leaseTime, string? name) : IReolinkAuthToken
{
    public double? LeaseTime { get; } = leaseTime;

    public string? Name { get; } = name;
}