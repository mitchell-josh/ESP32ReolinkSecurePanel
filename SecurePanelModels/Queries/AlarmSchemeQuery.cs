using SecurePanelModels.AlarmScheme;

namespace SecurePanelModels.Queries;

/// <summary>
/// A criteria object used to fetch specific alarm configurations from the database or cache.
/// Supports filtering by unique ID, broad security mode, or specific camera channel.
/// </summary>
public class AlarmSchemeQuery
{
    /// <summary>
    /// Filters by the unique primary key of the configuration record.
    /// </summary>
    public int? AlarmSchemeId { get; set; }
    
    /// <summary>
    /// Filters configurations belonging to a specific mode (e.g., fetch all "FullAlarm" settings).
    /// </summary>
    public AlarmSchemeTypes? AlarmSchemeType { get; set; }
    
    /// <summary>
    /// Filters configurations belonging to a specific camera (e.g., fetch all modes for "Front Door").
    /// </summary>
    public int? ChannelId { get; set; }
}