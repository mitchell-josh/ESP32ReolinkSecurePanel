using System.Net.Http.Json;
using System.Text.Json;

namespace ReolinkAPI.Utils;

public static class HttpUtils
{
    private static readonly JsonSerializerOptions JsonSerialiserOptions = new()
    {
        PropertyNamingPolicy = null // This ensures it uses the exact names/attributes
    };

    public static Task<HttpResponseMessage> PostAsJsonAsyncSafe<TEntity>(
        this HttpClient httpClient, 
        string requestUri, 
        TEntity entity) => httpClient.PostAsJsonAsync(requestUri, entity, JsonSerialiserOptions);

    public static List<TEntity> CreatePayloadArray<TEntity>(this TEntity entity) where TEntity: class => [entity];
}