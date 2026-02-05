namespace SecurePanelAPI.Handlers;

public class AlarmCodeLoggingHandler(ILogger<AlarmCodeLoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Content != null)
        {
            var requestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            logger.LogDebug("Auth Request: {RequestBody}", requestBody);
        }

        var response = await base.SendAsync(request, cancellationToken);
        
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogDebug("Auth Response: {ResponseBody}", responseBody);
        return response;
    }
}