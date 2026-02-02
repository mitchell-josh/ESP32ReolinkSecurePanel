using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReolinkAPI.Clients;
using SecurePanelAPI.Utils;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AudioAlarmController(ReolinkClient reolinkClient) : ControllerBase
{
    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpGet]
    public async Task<IActionResult> GetAudioAlarm([FromQuery] int channel)
    {
        try
        {
            var audioAlarm = await reolinkClient.GetAudioAlarm(channel);
            return Ok(audioAlarm);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}