using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the parameter container for authentication requests.
/// This acts as the wrapper for user credentials during the login process.
/// </summary>
public record ReolinkAuthParam(
    /* Gets or sets the user credentials, including username and password.
    Maps to the "User" key in the Reolink login JSON payload. */
    [property: JsonPropertyName("User")] ReoLinkAuthUser? User);