using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReolinkAPI.Clients;
using SecurePanelAPI.Utils;
using SecurePanelModels.DTOs;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AudioAlarmController(IAudioAlarmService audioAlarmService) : ControllerBase
{
    public async Task<IActionResult> UpdateAudioAlarm([FromBody] AlarmSettingsDto alarmSettings)
    {
        try
        {
            await audioAlarmService.UpdateAudioAlarm(alarmSettings);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}