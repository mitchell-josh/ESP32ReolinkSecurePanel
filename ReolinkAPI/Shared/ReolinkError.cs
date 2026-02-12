namespace ReolinkAPI.Shared;

/// <summary>
/// Contains detailed error information returned by the Reolink device when a command fails.
/// </summary>
public class ReolinkError
{
    /// <summary>
    /// A human-readable description of the error (e.g., "please login first", "not support").
    /// </summary>
    public string Detail { get; set; } = string.Empty;
    
    /// <summary>
    /// The specific hardware/firmware error code.
    /// </summary>
    public int RspCode { get; set; }
}