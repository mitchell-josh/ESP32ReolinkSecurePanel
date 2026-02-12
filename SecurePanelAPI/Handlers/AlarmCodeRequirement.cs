using Microsoft.AspNetCore.Authorization;

namespace SecurePanelAPI.Handlers;

/// <summary>
/// A marker class representing the requirement that a user must provide 
/// a valid Alarm Code (PIN) via request headers to access the resource.
/// </summary>
public class AlarmCodeRequirement : IAuthorizationRequirement
{
    // This class is purposefully empty. Its existence as a Type is what 
    // the Authorization Policy uses to trigger the AlarmCodeHandler.
}