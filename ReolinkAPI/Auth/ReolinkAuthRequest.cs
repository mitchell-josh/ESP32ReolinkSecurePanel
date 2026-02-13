using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the root request object for the Reolink "Login" command.
/// This is typically the first request sent to a device to establish a session.
/// </summary>
public record ReolinkAuthRequest(
    // Gets or sets the authentication parameters, which wrap the user credentials.
    ReolinkAuthParam? Param) : BaseRequest("Login", 0);