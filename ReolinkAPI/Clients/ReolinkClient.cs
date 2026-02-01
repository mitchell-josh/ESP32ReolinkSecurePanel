using System.Net.Http.Json;
using System.Text.Json;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Channels;
using ReolinkAPI.Utils;
using SecurePanelModels.Utils;

namespace ReolinkAPI.Clients;

public class ReolinkClient(HttpClient client, ISettings settings)
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
}