using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Audio;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Clients;
using ReolinkAPI.Shared;
using ReolinkAPI.Utils;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.DTOs;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

public class AudioAlarmService(ReolinkClient reolinkClient, SecurePanelDbContext db) : IAudioAlarmService
{
    public async Task<bool> UpdateAudioAlarm(int channelId)
    {
        var scheme = this.GetScheme(channelId);
        
        return await reolinkClient.SetAudioAlarm(GenerateSetAudioRequest(scheme));
    }

    private static SetAudioAlarmRequest GenerateSetAudioRequest(AlarmScheme scheme)
    {
        return new SetAudioAlarmRequest
        {
            Param = new SetAudioAlarmParam
            {
                Audio = new AudioAlarm
                {
                    StopAlarm = 0,
                    Enable = scheme.Enabled ? 1 : 0,
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

    private AlarmScheme GetScheme(int channelId)
        => this.GetChannel(channelId)
            .AlarmSchemes.OrderByDescending(s => s.DateCreated).First();
    
    private AlarmChannel GetChannel(int channelId)
        => db.AlarmChannels
            .Include(c => c.AlarmSchemes)
            .ThenInclude(s => s.AlarmSchedule)
            .Single(c => c.Identifier == channelId);
}