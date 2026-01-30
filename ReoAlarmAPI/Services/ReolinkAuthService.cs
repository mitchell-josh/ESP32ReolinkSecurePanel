using Microsoft.Extensions.Caching.Memory;
using Reolink.Auth;

namespace ReoAlarmAPI.Services;

public class ReolinkAuthService(HttpClient client, IMemoryCache memoryCache)
{
    public async Task<string> GetAuthTokenAsync()
    {
        if (memoryCache.TryGetValue("ReolinkAuthToken", out string? token))
        {
            return token ?? throw new ArgumentNullException(nameof(token));
        }

        var loginData = new[]
        {
            new ReolinkAuthRequest("0,", "admin", "123")
        };

        var response = await client.PostAsJsonAsync("api.cgi?cmd=Login", loginData);

        var result = await response.Content.ReadFromJsonAsync<List<ReolinkAuthResponse>>();

        var newToken = result?[0]?.Token;

        memoryCache.Set("ReolinkAuthToken", newToken?.Name, TimeSpan.FromSeconds(newToken?.LeaseTime ?? 0));

        return memoryCache.Get("ReolinkAuthToken") as string ?? string.Empty;
    }
}