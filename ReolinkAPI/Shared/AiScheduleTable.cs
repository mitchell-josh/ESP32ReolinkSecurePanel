using System.Text.Json.Serialization;

namespace ReolinkAPI.Shared;

public class AiScheduleTable
{
    [JsonPropertyName("AI_DOG_CAT")]
    public string? AiDogCat { get; set; }
    
    [JsonPropertyName("AI_OTHER")]
    public string? AiOther { get; set; }
    
    [JsonPropertyName("AI_PEOPLE")]
    public string? AiPeople { get; set; }
    
    [JsonPropertyName("AI_VEHICLE")]
    public string? AiVehicle { get; set; }
}