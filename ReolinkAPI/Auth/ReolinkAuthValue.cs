using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the value container returned in a successful <see cref="ReolinkAuthResponse"/>.
/// This class acts as a wrapper for the session token details.
/// </summary>
public record ReolinkAuthValue(
    /* Gets or sets the authentication token information.
     Maps to the "Token" key in the Reolink login JSON response. */
    [property: JsonPropertyName("Token")] ReolinkAuthToken? Token);