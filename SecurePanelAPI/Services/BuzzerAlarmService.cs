using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Audio;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Clients;
using ReolinkAPI.Handlers;
using ReolinkAPI.Shared;
using ReolinkAPI.Utils;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

/// <summary>
/// Manages the NVR's internal buzzer (the beep hardware inside the recorder).
/// Typically used to alert occupants inside the building of an event.
/// </summary>
public class BuzzerAlarmService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IBuzzerAlarmService
{
    public async Task<AlarmResult<bool>> UpdateBuzzerAlarm(AlarmSchemeQuery query)
    {
        // Get local scheme data
        var scheme = await this.GetScheme(query);
        if (scheme == null)
        {
            return AlarmResult<bool>.Failure("Scheme not found.");
        }
        
        var raw = await reolinkClient.SetBuzzerAlarm(GenerateSetBuzzerRequest(scheme));
        
        var result = ReolinkHandler.ProcessResponse<BuzzerAlarmResponse>(raw);

        return !result.Succeeded ? AlarmResult<bool>.Failure(result.ErrorMessage!) : AlarmResult<bool>.Success(true);
    }

    /// <summary>
    /// Constructs the JSON payload for the Reolink CGI API.
    /// Note: Ensure the detection types match the specific properties of the AlarmSchedule.
    /// </summary>
    private SetBuzzerAlarmRequest GenerateSetBuzzerRequest(AlarmScheme scheme)
    {
        return new SetBuzzerAlarmRequest
        {
            Param = new SetBuzzerAlarmParam
            {
                Buzzer = new BuzzerAlarm
                {
                    Enable = scheme.Enabled ? 1 : 0,
                    ScheduleEnabled = scheme.Enabled ? 1 : 0,
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