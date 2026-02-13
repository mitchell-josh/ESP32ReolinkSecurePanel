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
public class ChannelsController(IChannelService channelService) : BaseController
{
    /// <summary>
    /// Retrieves a list of all discovered channels, including their names, 
    /// hardware identifiers, and online status.
    /// </summary>
    [Authorize(Policy = Consts.AlarmCodePolicy)]
    [HttpGet]
    public async Task<IActionResult> GetChannels() 
        => await this.ExecuteAsync(async () => await channelService.GetChannels());
}