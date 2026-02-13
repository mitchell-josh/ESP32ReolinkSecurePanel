using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using ReolinkAPI.Auth;
using ReolinkAPI.Utils;
using SecurePanelModels.Utils;

namespace ReolinkAPI.Services;

/// <summary>
/// Manages session tokens for Reolink devices, handling authentication and local caching.
/// </summary>
public class ReolinkAuthService(HttpClient client, IMemoryCache memoryCache, ISettings settings)
{
    public async Task<string> GetAuthTokenAsync()
    {
        // Check if we have a valid, non-expired token in memory
        if (memoryCache.TryGetValue("ReolinkAuthToken", out string? token))
        {
            return token ?? throw new ArgumentNullException(nameof(token));
        }
        
        // Prepare the login payload (wrapped in an array for Reolink's CGI)
        var requestPayload = GetRequestPayload(settings.Username, settings.Password)
            .CreatePayloadArray();
        
        // Post to the Login endpoint
        var response = await client.PostAsJsonAsyncSafe("api.cgi?cmd=Login", requestPayload);
        
        var rawJson = await response.Content.ReadAsStringAsync();
        
        // Unwrap the response array
        var result = HttpUtils.DeserialiseSafe<List<ReolinkAuthResponse>>(rawJson);
        
        var newToken = result?[0]?.Value?.Token;
        
        // Add 30 second buffer to lease time
        var leaseTime = Math.Max(0, (newToken?.LeaseTime ?? 0) - 30);

        memoryCache.Set("ReolinkAuthToken", newToken?.Name,TimeSpan.FromSeconds(leaseTime));

        return newToken?.Name!;
    }

    private static ReolinkAuthRequest GetRequestPayload(string? username, string? password) =>
        new(Param: new ReolinkAuthParam(User: new ReoLinkAuthUser("0", username, password)));
}