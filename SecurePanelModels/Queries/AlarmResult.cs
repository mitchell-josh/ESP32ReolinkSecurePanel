namespace SecurePanelModels.Queries;

/// <summary>
/// A standardized wrapper for all internal security panel operations.
/// Encapsulates the success status, the resulting data, and error messaging.
/// </summary>
/// <typeparam name="T">The type of data being returned (e.g., ChannelDto, PushValue).</typeparam>
public class AlarmResult<T>
{
    private AlarmResult(bool succeeded, T? value, string? error)
    {
        this.Succeeded = succeeded;
        this.Value = value;
        this.ErrorMessage = error;
    }
    
    /// <summary>
    /// Indicates if the operation was completed successfully.
    /// </summary>
    public bool Succeeded { get; set; }
    
    /// <summary>
    /// The payload of the result. Only guaranteed to be non-null if Succeeded is true.
    /// </summary>
    public T? Value { get; set; }
    
    /// <summary>
    /// The reason for failure, suitable for logging or UI display.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Factory method to create a successful result.
    /// </summary>
    public static AlarmResult<T> Success(T value)
        => new(true, value, null);

    /// <summary>
    /// Factory method to create a failed result.
    /// </summary>
    public static AlarmResult<T> Failure(string error)
        => new(false, default, error);
}