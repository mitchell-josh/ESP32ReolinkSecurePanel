namespace ReolinkAPI.Shared;

/// <summary>
/// The generic response container for all Reolink API interactions.
/// </summary>
/// <typeparam name="T">The type of the expected 'value' payload (e.g., PushValue, ChannelResponse).</typeparam>
public class ReolinkResult<T>
{
    /// <summary>
    /// The original command name returned by the device.
    /// Maps to "cmd" in the JSON response.
    /// </summary>
    public string Cmd { get; set; } = string.Empty;
    
    /// <summary>
    /// The top-level status code. 
    /// 0: Success, 1: Error (check Error property).
    /// </summary>
    public int Code { get; set; }
    
    /// <summary>
    /// Detailed error information, populated if Code is non-zero.
    /// </summary>
    public ReolinkError? Error { get; set; }
    
    /// <summary>
    /// The actual data payload returned by the camera.
    /// </summary>
    public T? Value { get; set; }   
}