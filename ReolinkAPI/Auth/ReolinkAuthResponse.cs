using ReolinkAPI.Shared;

namespace ReolinkAPI.Auth;

/// <summary>
/// Represents the response received from a Reolink device after a Login attempt.
/// Inherits from <see cref="BaseRequest"/> to provide access to the response status code.
/// </summary>
public record ReolinkAuthResponse(
    /* Gets or sets the authentication data returned by the device.
     This property contains the sesion token required for subsequent API calls.*/
    ReolinkAuthValue? Value) : BaseRequest();