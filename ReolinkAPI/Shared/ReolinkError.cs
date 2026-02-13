namespace ReolinkAPI.Shared;

/// <summary>
/// Contains detailed error information returned by the Reolink device when a command fails.
/// </summary>
public record ReolinkError(
    // A human-readable description of the error (e.g., "please login first")
    string Detail,
    
    // The specific hardware/firmware error code
    int RspCode);