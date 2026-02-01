using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.BuzzerAlarm;

public class BuzzerAlarm
{
    [JsonPropertyName("diskErrorAlert")]
    public int? DiskErrorAlert { get; set; }
    
    [JsonPropertyName("diskFullAlert")]
    public int? DiskFullAlert { get; set; }
    
    [JsonPropertyName("enable")]
    public int? Enable { get; set; }
    
    [JsonPropertyName("ipConfigAlert")]
    public int? IpConfigAlert { get; set; }
    
    [JsonPropertyName("nvrDisconnectAlert")]
    public int? NvrDisconnectAlert { get; set; }
    
    [JsonPropertyName("scheduleEnabled")]
    public int? ScheduleEnabled { get; set; }
    
    [JsonPropertyName("schedule")]
    public AiSchedule? Schedule { get; set; }
}