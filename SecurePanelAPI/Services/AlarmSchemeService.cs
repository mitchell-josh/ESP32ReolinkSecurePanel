using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Clients;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.AlarmScheme;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

public class AlarmSchemeService(SecurePanelDbContext db) : IAlarmSchemeService
{
    public async Task<AlarmResult<AlarmSchemeDto>> GetAlarmScheme(AlarmSchemeQuery query)
    {
        var scheme = await this.GetScheme(query);
        if (scheme != null)
        {
            return AlarmResult<AlarmSchemeDto>.Success(this.GetAlarmSchemeDto(scheme));
        }

        return AlarmResult<AlarmSchemeDto>.Success(this.GetAlarmSchemeDto(await this.GetDefaultScheme(query)));
    }

    public async Task<AlarmResult<bool>> SaveAlarmScheme(AlarmSchemeDto scheme)
    {
        if (scheme.Validate())
        {
            db.AlarmSchemes.Add(new AlarmScheme
            {
                AlarmChannelId = scheme.AlarmChannelId!.Value!,
                AlarmSchemeTypeId = scheme.AlarmSchemeTypeId!.Value,
                Enabled = scheme.Enabled ?? false,
                PushEnabled = scheme.PushEnabled ?? false,
                AlarmSchedule = new AlarmSchedule
                {
                    PetsEnabled = scheme.Schedule?.PetsEnabled ?? false,
                    OtherEnabled = scheme.Schedule?.OtherEnabled ?? false,
                    PeopleEnabled = scheme.Schedule?.PeopleEnabled ?? false,
                    VehicleEnabled = scheme.Schedule?.VehicleEnabled ?? false,
                },
                DateCreated = DateTime.UtcNow,
            });
            await db.SaveChangesAsync();
            return AlarmResult<bool>.Success(true);
        }
        return AlarmResult<bool>.Failure("Invalid alarm scheme");
    }

    public async Task<AlarmResult<List<AlarmSchemeTypeDto>>> GetAlarmSchemeTypes()
    {
        var alarmSchemeTypes = await db.AlarmSchemeTypes.Select(t => new AlarmSchemeTypeDto
        {
            AlarmSchemeTypeId = t.AlarmSchemeTypeId,
            Key = t.Key,
        }).ToListAsync();

        return AlarmResult<List<AlarmSchemeTypeDto>>.Success(alarmSchemeTypes);
    }

    private AlarmSchemeDto GetAlarmSchemeDto(AlarmScheme scheme)
    {
        return new AlarmSchemeDto
        {
            AlarmSchemeId = scheme.AlarmSchemeId,
            AlarmChannelId = scheme.AlarmChannelId,
            AlarmSchemeTypeId = scheme.AlarmSchemeTypeId,
            Enabled = scheme.Enabled,
            PushEnabled = scheme.PushEnabled,
            Schedule = new AlarmScheduleDto
            {
                OtherEnabled = scheme.AlarmSchedule?.OtherEnabled ?? false,
                PeopleEnabled = scheme.AlarmSchedule?.PeopleEnabled ?? false,
                PetsEnabled = scheme?.AlarmSchedule?.PetsEnabled ?? false,
                VehicleEnabled = scheme?.AlarmSchedule?.VehicleEnabled ?? false,
            }
        };
    }

    private async Task<AlarmScheme> GetDefaultScheme(AlarmSchemeQuery query)
        => new()
        {
            AlarmChannelId = query.ChannelId!.Value,
            AlarmSchemeTypeId = (await this.GetAlarmSchemeType(query)).AlarmSchemeTypeId,
            AlarmSchedule = this.GetDefaultSchedule(),
            DateCreated = DateTime.UtcNow,
            AlarmScheduleId = 0,
            Enabled = false,
            PushEnabled = false,
        };

    private AlarmSchedule GetDefaultSchedule()
        => new()
        {
            PetsEnabled = false,
            OtherEnabled = false,
            PeopleEnabled = false,
            VehicleEnabled = false,
        };

    private async Task<AlarmSchemeType> GetAlarmSchemeType(AlarmSchemeQuery query)
    {
        string? alarmSchemeType = query.AlarmSchemeType!.ToString();
        return await db.AlarmSchemeTypes
            .SingleAsync(t => t.Key == alarmSchemeType);
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