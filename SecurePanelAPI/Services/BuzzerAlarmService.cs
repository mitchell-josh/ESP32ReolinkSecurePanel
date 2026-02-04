using Microsoft.EntityFrameworkCore;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Clients;
using ReolinkAPI.Shared;
using ReolinkAPI.Utils;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.DTOs;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

public class BuzzerAlarmService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IBuzzerAlarmService
{
    public async Task<bool> UpdateBuzzerAlarm(int channelId)
    {
        var scheme = this.GetScheme(channelId);
        return await reolinkClient.SetBuzzerAlarm(GenerateSetBuzzerRequest(scheme));
    }

    private SetBuzzerAlarmRequest GenerateSetBuzzerRequest(AlarmScheme scheme)
    {
        return new SetBuzzerAlarmRequest
        {
            Param = new SetBuzzerAlarmParam
            {
                Buzzer = new BuzzerAlarm
                {
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

    private AlarmScheme GetScheme(int channelId)
        => this.GetChannel(channelId)
            .AlarmSchemes.OrderByDescending(s => s.DateCreated).First();
    
    private AlarmChannel GetChannel(int channelId)
        => db.AlarmChannels
            .Include(c => c.AlarmSchemes)
            .Include(c => c.AlarmSchemes.Select(s => s.AlarmSchedule))
            .Single(c => c.Identifier == channelId);
}