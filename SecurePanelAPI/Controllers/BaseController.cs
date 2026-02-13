using Microsoft.AspNetCore.Mvc;
using SecurePanelModels.Queries;

namespace SecurePanelAPI.Controllers;

/// <summary>
/// SecurePanelAPI base controller class, inherits from <see cref="ControllerBase"/>
/// </summary>
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Generic wrapper for controller actions.
    /// </summary>
    protected async Task<IActionResult> ExecuteAsync<T>(Func<Task<T>> action)
    {
        try
        {
            var result = await action();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Ok(AlarmResult<bool>.Failure($"Internal Server Error: {ex.Message}"));
        }
    }
}