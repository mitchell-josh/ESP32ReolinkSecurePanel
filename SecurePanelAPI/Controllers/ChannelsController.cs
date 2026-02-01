using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReolinkAPI.Clients;
using SecurePanelAPI.Utils;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChannelsController(ReolinkClient reolinkClient) : ControllerBase
{
    [Authorize(Policy = Consts.AlarmCodePolicy)]
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