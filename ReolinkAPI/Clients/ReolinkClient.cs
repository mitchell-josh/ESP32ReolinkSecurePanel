using System.Net.Http.Json;
using System.Text.Json;
using ReolinkAPI.Audio;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Channels;
using ReolinkAPI.Push;
using ReolinkAPI.Shared;
using ReolinkAPI.Utils;
using SecurePanelModels.Utils;

namespace ReolinkAPI.Clients;

/// <summary>
/// The primary client for interacting with Reolink device APIs.
/// Requires an HttpClient pre-configured with the <see cref="ReolinkAuthClient"/> handler.
/// </summary>
public class ReolinkClient(HttpClient client)
{
    public async Task<ReolinkResult<ChannelResponse>> GetChannelStatus()
        => await this.PostAsync<ChannelResponse>(
            "api.cgi?cmd=GetChannelStatus", 
            new ChannelRequest(new EmptyParam()));

    public async Task<ReolinkResult<BuzzerAlarmResponse>> GetBuzzerAlarm(int channel)
        => await this.PostAsync<BuzzerAlarmResponse>(
            "api.cgi?cmd=GetBuzzerAlarmV20", 
            HttpUtils.CreateBuzzerAlarmRequestPayload(channel));
    
    public async Task<ReolinkResult<BuzzerAlarmResponse>> SetBuzzerAlarm(SetBuzzerAlarmRequest request)
        => await this.PostAsync<BuzzerAlarmResponse>(
            "api.cgi?cmd=SetBuzzerAlarmV20", 
            request);
    
    public async Task<ReolinkResult<AudioAlarmResponse>> GetAudioAlarm(int channel) 
        => await this.PostAsync<AudioAlarmResponse>(
            "api.cgi?cmd=GetAudioAlarmV20", 
            HttpUtils.CreateAudioAlarmRequestPayload(channel));

    public async Task<ReolinkResult<AudioAlarmResponse>> SetAudioAlarm(SetAudioAlarmRequest request)
        => await this.PostAsync<AudioAlarmResponse>(
            "api.cgi?cmd=SetAudioAlarmV20", 
            request);

    public async Task<ReolinkResult<PushResponse>> SetPush(SetPushRequest request) 
        => await this.PostAsync<PushResponse>(
            "api.cgi?cmd=SetPushV20", 
            request);

    /// <summary>
    /// Sends a POST request to the camera, wrapping the payload in an array 
    /// and unwrapping the first result from the response array.
    /// </summary>
    private async Task<ReolinkResult<T>> PostAsync<T>(string requestUri, object payload)
    {
        var body = payload.CreatePayloadArray();
        
        var response = await client.PostAsJsonAsyncSafe(requestUri, body);
        response.EnsureSuccessStatusCode();
        
        var rawJson = await response.Content.ReadAsStringAsync();
        
        var result = HttpUtils.DeserialiseSafe<List<ReolinkResult<T>>>(rawJson);
        
        return result!.First();
    }
}