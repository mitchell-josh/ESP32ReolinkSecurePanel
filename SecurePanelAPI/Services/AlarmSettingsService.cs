using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Clients;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.AlarmScheme;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

public class AlarmSettingsService(SecurePanelDbContext db) : IAlarmSettingsService
{
    public async Task<AlarmSettingsDto> GetAlarmSettings(AlarmSchemeQuery query)
    {
        var alarmSettings = await this.GetAlarmSettings(query.Channel) 
                            ?? await this.GetDefaultAlarmSettings(query);
        
        return this.MapAlarmSettingsDto(alarmSettings!);
    }

    public async Task SaveAlarmSettings(AlarmSettingsDto settings)
    {
        db.Set<AlarmSettings>().Add(new AlarmSettings
        {
            Channel = new Channel
            {
                Key = settings.ChannelId,
                Name = settings.ChannelName
            },
            AlarmScheme = new AlarmScheme
            {
                AlarmSchemeType = new AlarmSchemeType
                {
                    Key = settings.AlarmSchemeType.ToString()
                }
            },
            AlarmSchedule = new AlarmSchedule
            {
                AiDogCat = settings.AiSchedule.PeopleEnabled!.Value!,
                AiOther = settings.AiSchedule.OtherEnabled!.Value!,
                AiPeople = settings.AiSchedule.PeopleEnabled!.Value!,
                AiVehicle = settings.AiSchedule.CarsEnabled!.Value!
            }
        });
        await db.SaveChangesAsync();
    }

    private AlarmSettingsDto MapAlarmSettingsDto(AlarmSettings alarmSettings) 
        => new()
        {
            ChannelId = alarmSettings.ChannelId,
            Enabled = alarmSettings.Enabled,
            AlarmSchemeType = Enum.Parse<AlarmSchemeTypes>(alarmSettings.AlarmScheme!.AlarmSchemeType!.Key!),
            AiSchedule = new AiScheduleDto
            {
                PetsEnabled = alarmSettings.AlarmSchedule!.AiDogCat,
                PeopleEnabled = alarmSettings.AlarmSchedule!.AiPeople,
                CarsEnabled = alarmSettings.AlarmSchedule!.AiVehicle,
                OtherEnabled = alarmSettings.AlarmSchedule!.AiOther,   
            }
        };

    private async Task<AlarmSettings?> GetDefaultAlarmSettings(AlarmSchemeQuery query) 
        => new()
        {
            Enabled = false, // default to disabled
            Channel = await this.GetChannel(query),
            AlarmScheme = await this.GetAlarmScheme(query),
            AlarmSchedule = this.GetDefaultAlarmSchedule(query),
        };

    private AlarmSchedule GetDefaultAlarmSchedule(AlarmSchemeQuery query)
    {
        return new AlarmSchedule
        {
            AiDogCat = false,
            AiOther = false,
            AiPeople = false,
            AiVehicle = false,
        };
    }

    private async Task<Channel?> GetChannel(AlarmSettingsDto dto)
        => await this.GetChannel(new AlarmSchemeQuery
        {
            AlarmSchemeType = dto.AlarmSchemeType,
            Channel = dto.ChannelId!.Value!,
        });

    private async Task<AlarmScheme?> GetAlarmScheme(AlarmSettingsDto dto)
        => await this.GetAlarmScheme((new AlarmSchemeQuery
        {
            AlarmSchemeType = dto.AlarmSchemeType,
            Channel = dto.ChannelId!.Value!
        }));
    
    private async Task<Channel?> GetChannel(AlarmSchemeQuery query) 
        => await db.Channels.SingleOrDefaultAsync(c => c.ChannelId == query.Channel);

    private async Task<AlarmScheme?> GetAlarmScheme(AlarmSchemeQuery query) 
        => await db.AlarmSchemes
            .Include(a => a.AlarmSchemeType)
            .SingleOrDefaultAsync(a => a.AlarmSchemeType!.Key == nameof(query.AlarmSchemeType));

    private async Task<AlarmSettings?> GetAlarmSettings(int channelId)
        => await db.AlarmSettings
            .Include(s => s.Channel)
            .Include(s => s.AlarmScheme)
            .SingleOrDefaultAsync(s => s.ChannelId == channelId);
}