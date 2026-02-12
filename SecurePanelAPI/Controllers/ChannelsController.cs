using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReolinkAPI.Clients;
using SecurePanelAPI.Utils;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Controllers;

/// <summary>
/// Provides access to the inventory of physical camera channels managed by the system.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public class ChannelsController(IChannelService channelService) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of all discovered channels, including their names, 
    /// hardware identifiers, and online status.
    /// </summary>
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