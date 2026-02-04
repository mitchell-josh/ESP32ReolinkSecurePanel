using System.Net.Http.Json;
using System.Text.Json;
using ReolinkAPI.Audio;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Channels;
using ReolinkAPI.Utils;
using SecurePanelModels.Utils;

namespace ReolinkAPI.Clients;

public class ReolinkClient(HttpClient client)
{
    public async Task<ChannelResponse?> GetChannelStatus()
    {
        var requestPayload = new ChannelRequest().CreatePayloadArray();
        
        var response = await client.PostAsJsonAsyncSafe("api.cgi?cmd=GetChannelStatus", requestPayload);
        
        var rawJson = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<ChannelResponse>>(rawJson)?.FirstOrDefault();
    }

    public async Task<BuzzerAlarmResponse?> GetBuzzerAlarm(int channel)
    {
        var requestPayload = GetBuzzerAlarmRequest.CreatePayload(channel).CreatePayloadArray();

        var response = await client.PostAsJsonAsyncSafe("api.cgi?cmd=GetBuzzerAlarmV20", requestPayload);

        var rawJson = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<List<BuzzerAlarmResponse>>(rawJson)?.FirstOrDefault();
    }

    public async Task<bool> SetBuzzerAlarm(SetBuzzerAlarmRequest request)
    {
        var response = await client.PostAsJsonAsync("api.cgi?cmd=SetBuzzerAlarmV20", request.CreatePayloadArray());

        return true;
    }

    public async Task<Audio.AudioAlarmResponse?> GetAudioAlarm(int channel)
    {
        var requestPayload = GetAudioAlarmRequest.CreatePayload(channel).CreatePayloadArray();
        
        var response = await client.PostAsJsonAsyncSafe("api.cgi?cmd=GetAudioAlarmV20", requestPayload);

        var rawJson = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<List<Audio.AudioAlarmResponse>>(rawJson)?.FirstOrDefault();
    }

    public async Task<bool> SetAudioAlarm(SetAudioAlarmRequest request)
    {
        var response = await client.PostAsJsonAsync("api.cgi?cmd=SetAudioAlarmV20", request.CreatePayloadArray());

        return true;
    }
}