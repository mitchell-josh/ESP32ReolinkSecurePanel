using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReolinkAPI.Clients;
using SecurePanelAPI.Utils;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuzzerAlarmController(ReolinkClient reolinkClient) : ControllerBase
{
    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpGet]
    public async Task<IActionResult> GetBuzzerAlarm([FromQuery] int channel)
    {
        try
        {
            var buzzerAlarm = await reolinkClient.GetBuzzerAlarm(channel);
            return Ok(buzzerAlarm);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}