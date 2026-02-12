using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurePanelAPI.Utils;
using SecurePanelModels.AlarmScheme;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Controllers;

/// <summary>
/// Exposes endpoints to manage security profiles and trigger system-wide arming/disarming.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class AlarmSchemeController(IAlarmSchemeService alarmSchemeService) : ControllerBase
{
    /// <summary>
    /// Retrieves specific alarm configurations (e.g., Get 'Stay' mode settings for 'Front Door').
    /// Requires a valid Alarm Code via policy.
    /// </summary>
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
    
    /// <summary>
    /// Creates a new alarm configuration profile.
    /// </summary>
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

    /// <summary>
    /// The 'Global Command' endpoint. Changes the house mode (Away, Home, Disarmed)
    /// and synchronizes all physical cameras to match the stored DB configuration.
    /// </summary>
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