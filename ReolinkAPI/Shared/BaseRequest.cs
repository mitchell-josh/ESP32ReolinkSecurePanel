using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

/// <summary>
/// Provides a foundational structure for all Reolink API requests and responses.
/// Captures the command identifier and the operation result or action code.
/// </summary>
public abstract class BaseRequest()
{
    /// <summary>
    /// Initializes a new instance with optional command and code values.
    /// </summary>
    /// <param name="command">The Reolink CGI command name (e.g., "GetBuzzerAlarmV20").</param>
    /// <param name="code">The action index for requests or the return code for responses.</param>
    protected BaseRequest(string? command = null, int? code = null) : this()
    {
        this.Command = command;
        this.Code = code;
    }
    
    /// <summary>
    /// Gets or sets the command name.
    /// Maps to "cmd" in the JSON payload.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cmd")] 
    public virtual string? Command { get; set; }

    /// <summary>
    /// Gets or sets the status code or action type.
    /// Maps to "code" in the JSON payload.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("code")] 
    public virtual int? Code { get; set; }
}