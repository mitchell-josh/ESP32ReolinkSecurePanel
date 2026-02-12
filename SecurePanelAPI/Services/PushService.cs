using Microsoft.EntityFrameworkCore;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Clients;
using ReolinkAPI.Handlers;
using ReolinkAPI.Push;
using ReolinkAPI.Shared;
using ReolinkAPI.Utils;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

/// <summary>
/// Manages the Push Notification settings on the Reolink hardware.
/// Controls when the camera is allowed to send alerts to the Reolink Cloud/Mobile App.
/// </summary>
public class PushService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IPushService
{
    public async Task<AlarmResult<bool>> UpdatePush(AlarmSchemeQuery query)
    {
        // Get local scheme data
        var scheme = await this.GetScheme(query);
        if (scheme == null)
        {
            return AlarmResult<bool>.Failure("Scheme not found.");
        }
        
        var raw = await reolinkClient.SetPush(GenerateSetPushRequest(scheme));
        
        var result = ReolinkHandler.ProcessResponse<PushResponse>(raw);

        return !result.Succeeded ? AlarmResult<bool>.Failure(result.ErrorMessage!) : AlarmResult<bool>.Success(true);
    }

    /// <summary>
    /// Constructs the Push-specific request.
    /// Note: Similar to the BuzzerService, verify if you want to map all detection 
    /// types to 'PetsEnabled' or separate them by category.
    /// </summary>
    private static SetPushRequest GenerateSetPushRequest(AlarmScheme scheme)
    {
        return new SetPushRequest
        {
            Param = new SetPushParam
            {
                Push = new PushValue
                {
                    Enable = scheme.PushEnabled ? 1 : 0,
                    ScheduleEnable = 1,
                    Schedule = new AiSchedule
                    {
                        Channel = scheme.AlarmChannel!.Identifier,
                        Table = new AiScheduleTable
                        {
                            AiDogCat = HttpUtils.GetSchedule(scheme?.AlarmSchedule?.PetsEnabled ?? false),
                            AiOther = HttpUtils.GetSchedule(scheme?.AlarmSchedule?.PetsEnabled ?? false),
                            AiPeople = HttpUtils.GetSchedule(scheme?.AlarmSchedule?.PetsEnabled ?? false),
                            AiVehicle = HttpUtils.GetSchedule(scheme?.AlarmSchedule?.PetsEnabled ?? false),
                        }
                    }
                }
            }
        };
    }
    
    private async Task<AlarmScheme?> GetScheme(AlarmSchemeQuery query)
    {
        string? alarmSchemeType = query.AlarmSchemeType!.ToString()!;
        return (await this.GetChannel(query.ChannelId!.Value))
            ?.AlarmSchemes
            ?.Where(s => s.AlarmSchemeType!.Key == alarmSchemeType)
            ?.OrderByDescending(s => s.DateCreated)
            ?.FirstOrDefault();
    }
    
    private async Task<AlarmChannel?> GetChannel(int channelId)
        => await db.AlarmChannels
            .Include(c => c.AlarmSchemes)
            .ThenInclude(s => s.AlarmSchedule)
            .Include(c => c.AlarmSchemes)
            .ThenInclude(s => s.AlarmSchemeType)
            .SingleOrDefaultAsync(c => c.AlarmChannelId == channelId);
}