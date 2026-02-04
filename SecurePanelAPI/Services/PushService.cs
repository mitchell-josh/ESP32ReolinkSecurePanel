using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Clients;
using ReolinkAPI.Push;
using ReolinkAPI.Shared;
using ReolinkAPI.Utils;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.DTOs;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

public class PushService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IPushService
{
    public async Task<bool> UpdatePush(int channelId)
    {
        var scheme = this.GetScheme(channelId);
        return await reolinkClient.SetPush(GenerateSetPushRequest(scheme));
    }

    private static SetPushRequest GenerateSetPushRequest(AlarmScheme scheme)
    {
        return new SetPushRequest
        {
            Param = new SetPushParam
            {
                Push = new PushValue
                {
                    Enable = scheme.PushEnabled,
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
    
    private AlarmScheme GetScheme(int channelId)
        => this.GetChannel(channelId)
            .AlarmSchemes.OrderByDescending(s => s.DateCreated).First();
    
    private AlarmChannel GetChannel(int channelId)
        => db.AlarmChannels
            .Include(c => c.AlarmSchemes)
            .ThenInclude(s => s.AlarmSchedule)
            .Single(c => c.AlarmChannelId == channelId);
}