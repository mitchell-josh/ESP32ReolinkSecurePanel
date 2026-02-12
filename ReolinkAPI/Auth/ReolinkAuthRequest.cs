using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the root request object for the Reolink "Login" command.
/// This is typically the first request sent to a device to establish a session.
/// </summary>
public class ReolinkAuthRequest() : BaseRequest("Login")
{
    /// <summary>
    /// Gets or sets the authentication parameters, which wrap the user credentials.
    /// </summary>
    [JsonPropertyName("param")]
    public ReolinkAuthParam? Param { get; set; }
}