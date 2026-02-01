using Microsoft.AspNetCore.Mvc;
using ReoAlarmAPI.Clients;

namespace ReoAlarmAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScenesController(ReolinkClient reolinkClient) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetScenes()
    {
        try
        {
            var channels = await reolinkClient.GetChannelStatus();
            return Ok(channels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}