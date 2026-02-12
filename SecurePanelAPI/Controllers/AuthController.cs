using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurePanelAPI.Models;
using SecurePanelAPI.Services;
using SecurePanelAPI.Utils;
using SecurePanelModels.Queries;

namespace SecurePanelAPI.Controllers;

/// <summary>
/// Handles authentication logic and credential management for the security panel.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IAlarmCodeService alarmCodeService) : ControllerBase
{
    /// <summary>
    /// Simple heartbeat/smoke-test endpoint to verify the API is alive.
    /// </summary>
    [HttpGet]
    public IActionResult Test()
    {
        return Ok(AlarmResult<bool>.Success(true));
    }
    
    /// <summary>
    /// Updates the user's PIN. 
    /// Secured by the AlarmCodePolicy to ensure only an authenticated user can change their own code.
    /// </summary>
    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpPost]
    public async Task<IActionResult> ChangeAlarmCode([FromQuery] string newAlarmCode)
    {
        try
        {
            var username = this.User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(this.User.Identity?.Name))
            {
                return Unauthorized();
            }
            
            var result = await alarmCodeService.ChangeAlarmCode(this.User.Identity.Name, newAlarmCode);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates a PIN. This is typically used by the frontend Keypad 
    /// to obtain a session token or verify access before showing the dashboard.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CheckAlarmCode([FromQuery] string alarmCode)
    {
        try
        {
            var result = await alarmCodeService.CheckAlarmCode("Admin", alarmCode);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}