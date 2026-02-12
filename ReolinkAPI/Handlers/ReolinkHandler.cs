using ReolinkAPI.Shared;
using SecurePanelModels.Queries;

namespace ReolinkAPI.Handlers;

/// <summary>
/// Provides utility methods to translate raw Reolink API results into standardized application outcomes.
/// </summary>
public static class ReolinkHandler
{
    /// <summary>
    /// Processes a <see cref="ReolinkResult{T}"/> and converts it into a domain-specific <see cref="AlarmResult{T}"/>.
    /// Handles null checks, success codes, and error string formatting.
    /// </summary>
    /// <typeparam name="T">The type of the expected response data.</typeparam>
    /// <param name="response">The raw result from the Reolink device.</param>
    /// <returns>A successful result containing the data, or a failure result with a formatted error message.</returns>
    public static AlarmResult<T> ProcessResponse<T>(ReolinkResult<T>? response)
    {
        if (response == null)
        {
            return AlarmResult<T>.Failure("No response returned.");
        }

        // Code 0 is universal success indicator for Reolink APIs
        if (response.Code == 0)
        {
            return AlarmResult<T>.Success(response.Value!);
        }

        // Extract and format error details
        var errorDetail = response.Error?.Detail?.Trim() ?? "Unknown error.";
        var errorCode = response.Error?.RspCode.ToString()?.Trim() ?? "-1";

        return AlarmResult<T>.Failure($"(Error Code: {errorCode}) {errorDetail}");
    } 
}