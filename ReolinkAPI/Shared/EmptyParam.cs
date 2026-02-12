namespace ReolinkAPI.Shared;

/// <summary>
/// Represents a placeholder for API commands that require a 'param' object 
/// but do not need any specific input values.
/// </summary>
/// <remarks>
/// Reolink's CGI protocol often expects the structure: { "cmd": "...", "param": {} }.
/// Providing an empty object instead of null ensures compatibility across firmware versions.
/// </remarks>
public class EmptyParam
{
    // This class is intentionally left blank to serialize as an empty JSON object: {}
}