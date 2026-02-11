using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurePanelAPI.Models;
using SecurePanelAPI.Services;
using SecurePanelAPI.Utils;
using SecurePanelModels.Queries;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IAlarmCodeService alarmCodeService) : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok(AlarmResult<bool>.Success(true));
    }
    
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