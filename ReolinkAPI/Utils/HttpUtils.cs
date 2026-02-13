using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ReolinkAPI.Audio;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Push;

namespace ReolinkAPI.Utils;

/// <summary>
/// Internal utilities for HTTP communication and data formatting.
/// </summary>
public static class HttpUtils
{
    /// <summary>
    /// Serialiser options
    /// </summary>
    private static readonly JsonSerializerOptions JsonSerialiserOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
    
    /// <summary>
    /// Factory method to quickly generate a request payload for a specific camera channel.
    /// </summary>
    /// <param name="channel">The index of the camera channel (usually 0).</param>
    /// <returns>A populated <see cref="GetAudioAlarmRequest"/> object.</returns>
    public static GetAudioAlarmRequest CreateAudioAlarmRequestPayload(int channel) =>
        new(Param: new GetAudioAlarmParam(channel));

    /// <summary>
    /// Factory method to generate a request for a specific camera channel.
    /// </summary>
    /// <param name="channel">The index of the camera channel (0 for most standalone cameras).</param>
    /// <returns>A configured <see cref="GetBuzzerAlarmRequest"/> ready for serialisation.</returns>
    public static GetBuzzerAlarmRequest CreateBuzzerAlarmRequestPayload(int channel) =>
        new(Param: new BuzzerAlarmParam(channel));

    /// <summary>
    /// Factory method to generate a request for a specific camera channel.
    /// </summary>
    public static GetPushRequest CreatePushRequestPayload(int channel) =>
        new(Param: new GetPushParam(channel));
}