using Microsoft.Extensions.Logging;

namespace ReolinkAPI.Handlers;

public class ReolinkLoggingHandler(ILogger<ReolinkLoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Content != null)
        {
            var requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            logger.LogDebug("Reolink Request: {RequestBody}", requestBody);
        }

        var response = await base.SendAsync(request, cancellationToken);
        
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogDebug("Reolink Response: {ResponseBody}", responseBody);
        return response;
    }
}