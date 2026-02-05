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

public class PushService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IPushService
{
    public async Task<AlarmResult<bool>> UpdatePush(AlarmSchemeQuery query)
    {
        // Get local scheme data
        var scheme = this.GetScheme(query);
        if (scheme == null)
        {
            return AlarmResult<bool>.Failure("Scheme not found.");
        }
        
        var raw = await reolinkClient.SetPush(GenerateSetPushRequest(scheme));
        
        var result = ReolinkHandler.ProcessResponse<PushResponse>(raw);

        return !result.Succeeded ? AlarmResult<bool>.Failure(result.ErrorMessage!) : AlarmResult<bool>.Success(true);
    }

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