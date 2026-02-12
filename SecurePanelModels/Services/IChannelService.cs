using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;

namespace SecurePanelModels.Services;

/// <summary>
/// Manages the discovery and retrieval of camera channels configured within the system.
/// </summary>
public interface IChannelService
{
    /// <summary>
    /// Fetches a list of all available camera channels, including their names, 
    /// hardware keys, and current enabled status.
    /// </summary>
    /// <returns>An AlarmResult containing a collection of ChannelDto objects.</returns>
    Task<AlarmResult<List<ChannelDto>>> GetChannels();
}