using Microsoft.Extensions.Logging;

namespace ReolinkAPI.Handlers;

/// <summary>
/// An HTTP message handler that logs the raw JSON request and response bodies.
/// Crucial for debugging hardware-specific command failures.
/// </summary>
public class ReolinkLoggingHandler(ILogger<ReolinkLoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Log outgoing request payload
        if (request.Content != null)
        {
            var requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            logger.LogDebug("Reolink Request: {RequestBody}", requestBody);
        }

        // Execute the request
        var response = await base.SendAsync(request, cancellationToken);
        
        // Log incoming request payload
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogDebug("Reolink Response: {ResponseBody}", responseBody);
        return response;
    }
}