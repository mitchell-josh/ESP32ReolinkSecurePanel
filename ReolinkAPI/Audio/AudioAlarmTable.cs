using System.Text.Json.Serialization;

namespace ReolinkAPI.Audio;

public class AudioAlarmTable
{
    [JsonPropertyName("AI_DOG_CAT")]
    public int[]? AiDogCat { get; set; }
    
    [JsonPropertyName("AI_OTHER")]
    public int[]? AiOther { get; set; }
    
    [JsonPropertyName("AI_PEOPLE")]
    public int[]? AiPeople { get; set; }
    
    [JsonPropertyName("AI_VEHICLE")]
    public int[]? AiVehicle { get; set; }
}