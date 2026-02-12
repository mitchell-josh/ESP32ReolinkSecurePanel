using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the parameter container for authentication requests.
/// This acts as the wrapper for user credentials during the login process.
/// </summary>
public class ReolinkAuthParam
{
    /// <summary>
    /// Gets or sets the user credentials, including username and password.
    /// Maps to the "User" key in the Reolink login JSON payload.
    /// </summary>
    /// <remarks>
    /// Note: Reolink's authentication JSON keys are often PascalCase ("User") 
    /// unlike their hardware settings which are usually camelCase.
    /// </remarks>
    [JsonPropertyName("User")]
    public ReoLinkAuthUser? User { get; set; }
}