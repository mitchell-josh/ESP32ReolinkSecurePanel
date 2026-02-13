using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Clients;
using SecurePanelAPI.Utils;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.AlarmScheme;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

/// <summary>
/// The primary service responsible for managing alarm configurations and 
/// pushing those configurations to physical hardware.
/// </summary>
public class AlarmSchemeService(
    SecurePanelDbContext db,
    IAudioAlarmService audioAlarmService,
    IBuzzerAlarmService buzzerAlarmService,
    IPushService pushService) : IAlarmSchemeService
{
    public async Task<AlarmResult<AlarmSchemeDto>> GetAlarmScheme(AlarmSchemeQuery query)
    {
        var scheme = await ApiHttpUtils.GetScheme(db, query);
        if (scheme != null)
        {
            return AlarmResult<AlarmSchemeDto>.Success(this.GetAlarmSchemeDto(scheme));
        }

        return AlarmResult<AlarmSchemeDto>.Success(this.GetAlarmSchemeDto(await this.GetDefaultScheme(query)));
    }

    /// <summary>
    /// Saves a new alarm profile. Includes automated versioning by using DateCreated 
    /// rather than updating existing records, allowing for a configuration history.
    /// </summary>
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

    /// <summary>
    /// Orchestrates the 'Global Arming' process. It fetches all configuration 
    /// profiles for a specific mode and pushes them to every hardware channel.
    /// </summary>
    public async Task<AlarmResult<bool>> SetAlarm(AlarmSchemeTypes alarmSchemeType)
    {
        var schemes = await this.GetSchemes(alarmSchemeType) ?? [];

        bool success = true;
        
        foreach (var scheme in schemes)
        {
            var audioAlarmResult = await audioAlarmService.UpdateAudioAlarm(this.GetAlarmSchemeQuery(scheme));
            var buzzerAlarmResult = await buzzerAlarmService.UpdateBuzzerAlarm(this.GetAlarmSchemeQuery(scheme));
            var pushResult = await pushService.UpdatePush(this.GetAlarmSchemeQuery(scheme));
            success &= audioAlarmResult.Succeeded &&  buzzerAlarmResult.Succeeded && pushResult.Succeeded;
        }
        
        return success ?
            AlarmResult<bool>.Success(true) :
            AlarmResult<bool>.Failure("Failed to set alarm");
    }

    private AlarmSchemeQuery GetAlarmSchemeQuery(AlarmScheme scheme)
        => new AlarmSchemeQuery()
        {
            AlarmSchemeId = scheme.AlarmSchemeId,
            ChannelId = scheme.AlarmChannelId,
            AlarmSchemeType = Enum.Parse<AlarmSchemeTypes>(scheme.AlarmSchemeType!.Key),
        };
    
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

    private async Task<AlarmScheme[]> GetSchemes(AlarmSchemeTypes alarmSchemeType)
    {
        string? type = alarmSchemeType.ToString();
        return await db.AlarmSchemes
            .Include(s => s.AlarmSchemeType)
            .Where(s => s.AlarmSchemeType!.Key == type)
            .OrderByDescending(s => s.DateCreated)
            .GroupBy(t => t.AlarmChannelId)
            .Select(g => g.First())
            .ToArrayAsync();
    }
}