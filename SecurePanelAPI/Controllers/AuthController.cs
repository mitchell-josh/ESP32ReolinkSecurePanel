using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurePanelAPI.Models;
using SecurePanelAPI.Services;
using SecurePanelAPI.Utils;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IAlarmCodeService alarmCodeService) : ControllerBase
{
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

    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpPost]
    public async Task<IActionResult> CheckAlarmCode([FromQuery] string alarmCode)
    {
        try
        {
            var username = this.User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(this.User.Identity?.Name))
            {
                return Unauthorized();
            }

            var result = await alarmCodeService.CheckAlarmCode(this.User.Identity.Name, alarmCode);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}