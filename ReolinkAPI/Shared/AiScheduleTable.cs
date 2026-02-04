using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

public class AiScheduleTable
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("enable")]
    public int? Enable { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AI_DOG_CAT")]
    public string? AiDogCat { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AI_OTHER")]
    public string? AiOther { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AI_PEOPLE")]
    public string? AiPeople { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AI_VEHICLE")]
    public string? AiVehicle { get; set; }
}