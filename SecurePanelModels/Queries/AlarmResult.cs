namespace SecurePanelModels.Queries;

public class AlarmResult<T>
{
    private AlarmResult(bool succeeded, T? value, string? error)
    {
        this.Succeeded = succeeded;
        this.Value = value;
        this.ErrorMessage = error;
    }
    
    public bool Succeeded { get; set; }
    
    public T? Value { get; set; }
    
    public string? ErrorMessage { get; set; }

    public static AlarmResult<T> Success(T value)
        => new(true, value, null);

    public static AlarmResult<T> Failure(string error)
        => new(false, default, error);
}