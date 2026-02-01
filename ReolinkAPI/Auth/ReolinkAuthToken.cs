using System.Text.Json.Serialization;

namespace ReolinkAPI.Auth;

public class ReolinkAuthToken
{
    [JsonPropertyName("leaseTime")]
    public double LeaseTime { get; set; }
        
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}