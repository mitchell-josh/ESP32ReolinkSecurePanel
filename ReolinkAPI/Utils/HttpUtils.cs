using System.Net.Http.Json;
using System.Text.Json;

namespace ReolinkAPI.Utils;

/// <summary>
/// Internal utilities for HTTP communication and data formatting.
/// </summary>
public static class HttpUtils
{
    private static readonly JsonSerializerOptions JsonSerialiserOptions = new()
    {
        PropertyNamingPolicy = null // This ensures it uses the exact names/attributes
    };

    /// <summary>
    /// Extension method to send a JSON POST request using the library's standard serialization settings.
    /// </summary>
    public static Task<HttpResponseMessage> PostAsJsonAsyncSafe<TEntity>(
        this HttpClient httpClient, 
        string requestUri, 
        TEntity entity) => httpClient.PostAsJsonAsync(requestUri, entity, JsonSerialiserOptions);

    /// <summary>
    /// Wraps a single object into a List for Reolink's array-based command format.
    /// Example: {cmd: "..."} becomes [{cmd: "..."}]
    /// </summary>
    public static List<TEntity> CreatePayloadArray<TEntity>(this TEntity entity) where TEntity: class => [entity];

    /// <summary>
    /// Generates a 168-character string (7 days * 24 hours) of '1's or '0's.
    /// </summary>
    /// <param name="enabled">True for 168 '1's (Always On), False for 168 '0's (Always Off).</param>
    public static string GetSchedule(bool enabled)
        => string.Join(string.Empty, Enumerable.Repeat(enabled ? 1 : 0, 168));
}