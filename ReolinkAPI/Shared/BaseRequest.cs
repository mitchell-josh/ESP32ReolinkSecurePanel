using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

/// <summary>
/// Provides a foundational structure for all Reolink API requests and responses.
/// Captures the command identifier and the operation result or action code.
/// </summary>
public abstract record BaseRequest(
    /* Gets or sets the command name.
    // Maps to "cmd" in the JSON payload. */
    [property: JsonPropertyName("cmd")] string? Command = null,

    /* Gets or sets the status code or action type.
    // Maps to "code" in the JSON payload. */
    int? Code = null);