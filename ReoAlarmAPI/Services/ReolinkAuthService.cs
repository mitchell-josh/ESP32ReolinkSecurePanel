using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using ReoAlarmModels.Utils;
using Reolink.Auth;
using ReoAlarmAPI.Utils;

namespace ReoAlarmAPI.Services;

public class ReolinkAuthService(HttpClient client, IMemoryCache memoryCache, ISettings settings)
{
    public async Task<string> GetAuthTokenAsync()
    {
        if (memoryCache.TryGetValue("ReolinkAuthToken", out string? token))
        {
            return token ?? throw new ArgumentNullException(nameof(token));
        }
        
        var requestPayload = GetRequestPayload(settings.Username, settings.Password)
            .CreatePayloadArray();
        
        var response = await client.PostAsJsonAsyncSafe("api.cgi?cmd=Login", requestPayload);
        
        var rawJson = await response.Content.ReadAsStringAsync();
        
        var result = JsonSerializer.Deserialize<List<ReolinkAuthResponse>>(rawJson);

        var newToken = result?[0]?.Value?.Token;
        
        // Add 30 second buffer to lease time
        var leaseTime = Math.Max(0, (newToken?.LeaseTime ?? 0) - 30);

        memoryCache.Set("ReolinkAuthToken", newToken?.Name,TimeSpan.FromSeconds(leaseTime));

        return newToken?.Name!;
    }
    
    private static ReolinkAuthRequest GetRequestPayload(string? username, string? password) =>
        new()
        {
            Param = new ReolinkAuthRequest.ReolinkAuthParam
            {
                User = new ReolinkAuthRequest.ReoLinkAuthUser
                {
                    Username = username!,
                    Password = password!
                }
            }
        };
}