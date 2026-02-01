using System.Text.Json;
using SecurePanelAPI.Utils;
using SecurePanelModels.Utils;
using ReolinkAPI.Channels;

namespace SecurePanelAPI.Clients;

public class ReolinkClient(HttpClient client, ISettings settings)
{
    public async Task<ChannelResponse?> GetChannelStatus()
    {
        var requestPayload = new ChannelRequest().CreatePayloadArray();
        
        var response = await client.PostAsJsonAsyncSafe("api.cgi?cmd=GetChannelStatus", requestPayload);
        
        var rawJson = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<ChannelResponse>>(rawJson)?.FirstOrDefault();
    }
}