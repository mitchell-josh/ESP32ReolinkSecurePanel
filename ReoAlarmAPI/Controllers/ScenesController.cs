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
            var scenes = await reolinkClient.GetScenesAsync();
            return Ok(scenes);
        }
        catch (Exception ex)
        {
            // Log the error as needed
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}