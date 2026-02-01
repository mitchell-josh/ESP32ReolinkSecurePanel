using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

public abstract class BaseRequest()
{
    protected BaseRequest(string? command = null, int? code = null) : this()
    {
        this.Command = command;
        this.Code = code;
    }
    
    [JsonPropertyName("cmd")] 
    public virtual string? Command { get; set; }

    [JsonPropertyName("code")] 
    public virtual int? Code { get; set; }
}