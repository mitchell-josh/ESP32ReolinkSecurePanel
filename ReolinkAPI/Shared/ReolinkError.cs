namespace ReolinkAPI.Shared;

public class ReolinkError
{
    public string Detail { get; set; } = string.Empty;
    
    public int RspCode { get; set; }
}