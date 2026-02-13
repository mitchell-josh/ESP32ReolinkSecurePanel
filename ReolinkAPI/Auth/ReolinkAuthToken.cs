using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the session token details provided by the device upon successful authentication.
/// </summary>
public record ReolinkAuthToken(
    // Gets or sets the duration (in seconds) that the current session remains valid.
    double LeaseTime,

    /* Gets or sets the session token string.
     Despite the JSON key being "name", this field contains the authorisation token
     Value should be appended to the URL of subsequent requests: ?token=YourTokenValue*/
    string? Name);