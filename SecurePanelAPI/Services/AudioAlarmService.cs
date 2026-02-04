using ReolinkAPI.Audio;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Clients;
using ReolinkAPI.Shared;
using ReolinkAPI.Utils;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.DTOs;

namespace SecurePanelAPI.Services;

public class AudioAlarmService(ReolinkClient reolinkClient, SecurePanelDbContext db)
{
    public async Task<bool> UpdateAudioAlarm(AlarmSettingsDto channel)
    {
        if (!channel.ChannelId.HasValue) return false;
        
        var audioAlarm = await reolinkClient.GetAudioAlarm(channel.ChannelId ?? -1);

        if (audioAlarm?.Value == null) return false;

        var result = await reolinkClient.SetAudioAlarm(GenerateSetAudioRequest(audioAlarm, channel));

        return result;
    }

    private static SetAudioAlarmRequest GenerateSetAudioRequest(AudioAlarmResponse currentSettings, AlarmSettingsDto channel)
    {
        return new SetAudioAlarmRequest
        {
            Param = new SetAudioAlarmParam
            {
                Audio = new AudioAlarm
                {
                    StopAlarm = 0,
                    Enable = (channel.Enabled ?? false) ? 1 : 0,
                    Schedule = new AiSchedule
                    {
                        Channel = channel.ChannelId,
                        Table = new AiScheduleTable
                        {
                            AiDogCat = HttpUtils.GetSchedule(channel?.AiSchedule.PetsEnabled ?? false),
                            AiOther = HttpUtils.GetSchedule(channel?.AiSchedule.OtherEnabled?? false),
                            AiPeople = HttpUtils.GetSchedule(channel?.AiSchedule.PeopleEnabled ?? false),
                            AiVehicle = HttpUtils.GetSchedule(channel?.AiSchedule.CarsEnabled ?? false),
                        }
                    }
                }
            }
        };
    }
}