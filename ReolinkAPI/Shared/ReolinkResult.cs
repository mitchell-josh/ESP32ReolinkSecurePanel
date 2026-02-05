namespace ReolinkAPI.Shared;

public class ReolinkResult<T>
{
    public string Cmd { get; set; } = string.Empty;
    
    public int Code { get; set; }
    
    public ReolinkError? Error { get; set; }
    
    public T? Value { get; set; }   
}