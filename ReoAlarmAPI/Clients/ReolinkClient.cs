using System.Text.Json;
using ReoAlarmModels.Utils;

namespace ReoAlarmAPI.Clients;

public class ReolinkClient(HttpClient client, ISettings settings)
{
    public async Task<List<ReolinkScene>> GetScenesAsync()
    {
        var payload = new[] { new { cmd = "GetAbility", action = 1, param = new { User = new { userName = settings.Username } } } };
    
        var response = await client.PostAsJsonAsync("api.cgi?cmd=GetAbility", payload);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        // Navigate the JSON to find the scene list
        // Note: The path might vary slightly by firmware (e.g., result[0].value.ability.scene.list)
        var sceneList = json[0].GetProperty("value").GetProperty("ability").GetProperty("scene").GetProperty("list");

        return JsonSerializer.Deserialize<List<ReolinkScene>>(sceneList.GetRawText());
    }

    public record ReolinkScene(int Id, string Name);
}