using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurePanelAPI.Utils;
using SecurePanelModels.AlarmScheme;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AlarmSchemeController(IAlarmSchemeService alarmSchemeService) : ControllerBase
{
    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpPost]
    public async Task<IActionResult> GetAlarmScheme([FromBody] AlarmSchemeQuery scheme)
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

    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpPost]
    public async Task<IActionResult> SaveAlarmScheme([FromBody] AlarmSchemeDto scheme)
    {
        try
        {
            var result = await alarmSchemeService.SaveAlarmScheme(scheme);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpPost]
    public async Task<IActionResult> SetAlarm([FromQuery] AlarmSchemeTypes? alarmSchemeType)
    {
        if (!alarmSchemeType.HasValue)
        {
            return Ok(AlarmResult<bool>.Failure("AlarmSchemeType is missing from query parameters."));
        }

        try
        {
            var result = await alarmSchemeService.SetAlarm(alarmSchemeType.Value);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}