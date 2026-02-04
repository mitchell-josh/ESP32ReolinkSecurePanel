using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReolinkAPI.Clients;
using SecurePanelAPI.Utils;
using SecurePanelModels.DTOs;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuzzerAlarmController(IBuzzerAlarmService buzzerAlarmService) : ControllerBase
{
    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpPost]
    public async Task<IActionResult> UpdateBuzzerAlarm([FromQuery] int channelId)
    {
        try
        {
            await buzzerAlarmService.UpdateBuzzerAlarm(channelId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}