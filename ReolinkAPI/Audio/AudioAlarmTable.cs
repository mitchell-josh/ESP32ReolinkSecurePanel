using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class AudioAlarmTable
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