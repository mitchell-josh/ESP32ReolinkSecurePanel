using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReolinkAPI.Clients;
using SecurePanelAPI.Utils;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ChannelsController(IChannelService channelService) : ControllerBase
{
    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpGet]
    public async Task<IActionResult> GetChannels()
    {
        try
        {
            var channels = await channelService.GetChannels();
            return Ok(channels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}