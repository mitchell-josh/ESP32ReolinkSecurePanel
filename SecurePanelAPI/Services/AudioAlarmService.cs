using System.Threading.Channels;
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

public class AudioAlarmService(
    ReolinkClient reolinkClient, 
    SecurePanelDbContext db) : IAudioAlarmService
{
    public async Task<AlarmResult<bool>> UpdateAudioAlarm(AlarmSchemeQuery query)
    {
        // Get local scheme data
        var scheme = await this.GetScheme(query);
        if (scheme == null)
        {
            return AlarmResult<bool>.Failure("Scheme not found.");
        }
        
        var raw = await reolinkClient.SetAudioAlarm(GenerateSetAudioRequest(scheme));
        
        var result = ReolinkHandler.ProcessResponse<AudioAlarmResponse>(raw);

        return !result.Succeeded ? AlarmResult<bool>.Failure(result.ErrorMessage!) : AlarmResult<bool>.Success(true);
    }

    private static SetAudioAlarmRequest GenerateSetAudioRequest(AlarmScheme scheme)
    {
        return new SetAudioAlarmRequest
        {
            Param = new SetAudioAlarmParam
            {
                Audio = new AudioAlarm
                {
                    Enable = scheme.Enabled ? 1 : 0,
                    StopAlarm = 0,
                    Schedule = new AiSchedule
                    {
                        Channel = scheme.AlarmChannel!.Identifier,
                        Table = new AiScheduleTable
                        {
                            AiDogCat = HttpUtils.GetSchedule(scheme?.AlarmSchedule?.PetsEnabled ?? false),
                            AiOther = HttpUtils.GetSchedule(scheme?.AlarmSchedule?.OtherEnabled?? false),
                            AiPeople = HttpUtils.GetSchedule(scheme?.AlarmSchedule?.PeopleEnabled ?? false),
                            AiVehicle = HttpUtils.GetSchedule(scheme?.AlarmSchedule?.VehicleEnabled ?? false),
                        }
                    }
                }
            }
        };
    }

    private async Task<AlarmScheme?> GetScheme(AlarmSchemeQuery query)
    { 
        string? alarmSchemeType = query.AlarmSchemeType!.ToString();
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