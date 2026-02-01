using Microsoft.AspNetCore.Mvc;
using SecurePanelAPI.Clients;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChannelsController(ReolinkClient reolinkClient) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChannels()
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