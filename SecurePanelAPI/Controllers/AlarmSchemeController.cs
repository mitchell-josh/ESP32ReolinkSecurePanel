using Microsoft.AspNetCore.Mvc;
using SecurePanelModels.DTOs;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlarmSchemeController(IAlarmSchemeService alarmSchemeService) : ControllerBase
{
    public async Task<IActionResult> GetAlarmScheme([FromBody] AlarmSchemeDto scheme)
    {
        try
        {
            var result = await alarmSchemeService.GetAlarmScheme(scheme);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    public async Task<IActionResult> SaveAlarmSchedule([FromBody] AlarmSchemeDto scheme)
    {
        try
        {
            await alarmSchemeService.SaveAlarmScheme(scheme);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}