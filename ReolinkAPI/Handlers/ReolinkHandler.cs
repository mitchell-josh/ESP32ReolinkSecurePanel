using ReolinkAPI.Shared;
using SecurePanelModels.Queries;

namespace ReolinkAPI.Handlers;

public static class ReolinkHandler
{
    public static AlarmResult<T> ProcessResponse<T>(ReolinkResult<T>? response)
    {
        if (response == null)
        {
            return AlarmResult<T>.Failure("No response returned.");
        }

        if (response.Code == 0)
        {
            return AlarmResult<T>.Success(response.Value!);
        }

        var errorDetail = response.Error?.Detail?.Trim() ?? "Unknown error.";
        var errorCode = response.Error?.RspCode.ToString()?.Trim() ?? "-1";

        return AlarmResult<T>.Failure($"(Error Code: {errorCode}) {errorDetail}");
    } 
}