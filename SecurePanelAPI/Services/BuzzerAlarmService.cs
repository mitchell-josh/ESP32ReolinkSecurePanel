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

public class BuzzerAlarmService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IBuzzerAlarmService
{
    public async Task<AlarmResult<bool>> UpdateBuzzerAlarm(AlarmSchemeQuery query)
    {
        // Get local scheme data
        var scheme = this.GetScheme(query);
        if (scheme == null)
        {
            return AlarmResult<bool>.Failure("Scheme not found.");
        }
        
        var raw = await reolinkClient.SetBuzzerAlarm(GenerateSetBuzzerRequest(scheme));
        
        var result = ReolinkHandler.ProcessResponse<BuzzerAlarmResponse>(raw);

        return !result.Succeeded ? AlarmResult<bool>.Failure(result.ErrorMessage!) : AlarmResult<bool>.Success(true);
    }

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

    private AlarmScheme? GetScheme(AlarmSchemeQuery query)
        => this.GetChannel(query.ChannelId)
            ?.AlarmSchemes
            ?.Where(s => s.AlarmSchemeTypeId == query.AlarmSchemeTypeId)
            ?.OrderByDescending(s => s.DateCreated).FirstOrDefault();
    
    private AlarmChannel? GetChannel(int? channelId)
        => db.AlarmChannels
            .Include(c => c.AlarmSchemes)
            .ThenInclude(s => s.AlarmSchedule)
            .SingleOrDefault(c => c.AlarmChannelId == channelId);
}