using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurePanelAPI.Models;
using SecurePanelAPI.Services;
using SecurePanelAPI.Utils;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAlarmCodeService alarmCodeService) : ControllerBase
{
    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpPost(nameof(ChangeAlarmCode))]
    public IActionResult ChangeAlarmCode([FromQuery] string newAlarmCode)
    {
        try
        {
            var username = this.User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(this.User.Identity?.Name))
            {
                return Unauthorized();
            }
            
            alarmCodeService.ChangeAlarmCode(this.User.Identity.Name, newAlarmCode);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}