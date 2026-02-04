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
    public async Task<bool> UpdateBuzzerAlarm(AlarmSettingsDto channel)
    {
        if (!channel.ChannelId.HasValue) return false;
        
        var buzzerAlarm = await reolinkClient.GetBuzzerAlarm(channel.ChannelId ?? -1);

        if (buzzerAlarm?.Value == null) return false;

        var result = await reolinkClient.SetBuzzerAlarm(GenerateSetBuzzerRequest(buzzerAlarm, channel));

        return result;
    }

    private static SetBuzzerAlarmRequest GenerateSetBuzzerRequest(BuzzerAlarmResponse currentSettings, AlarmSettingsDto channel)
    {
        return new SetBuzzerAlarmRequest
        {
            Param = new SetBuzzerAlarmParam
            {
                Buzzer = new BuzzerAlarm
                {
                    DiskErrorAlert = currentSettings.Value!.Buzzer!.DiskErrorAlert,
                    DiskFullAlert = currentSettings.Value!.Buzzer!.DiskFullAlert,
                    Enable = (channel.Enabled ?? false) ? 1 : 0,
                    IpConfigAlert = currentSettings.Value!.Buzzer!.IpConfigAlert,
                    NvrDisconnectAlert = currentSettings.Value!.Buzzer!.NvrDisconnectAlert,
                    ScheduleEnabled = 1,
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