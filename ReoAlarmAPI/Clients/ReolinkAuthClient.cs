using ReoAlarmAPI.Services;

namespace ReoAlarmAPI.Clients;

public class ReolinkAuthClient(ReolinkAuthService authService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        // Get the token (AuthService should handle caching internally)
        var token = await authService.GetAuthTokenAsync();

        // Reolink usually wants the token in the query string: &token=xyz
        var uriBuilder = new UriBuilder(request.RequestUri!);
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        query["token"] = token;
        uriBuilder.Query = query.ToString();
        request.RequestUri = uriBuilder.Uri;

        // 3. Continue the request
        return await base.SendAsync(request, ct);
    }
}