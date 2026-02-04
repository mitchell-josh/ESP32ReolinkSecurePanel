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
    public async Task<AlarmSchemeDto> GetAlarmScheme(AlarmSchemeDto schemeDto) 
        => this.GetAlarmSchemeDto((await this.GetScheme(schemeDto)) ?? this.GetDefaultScheme(schemeDto));

    public async Task SaveAlarmScheme(AlarmSchemeDto scheme)
    {
        db.AlarmSchemes.Add(new AlarmScheme
        {
            AlarmChannelId = scheme.AlarmChannelId!.Value!,
            AlarmSchemeTypeId = scheme.AlarmSchemeTypeId!.Value,
            Enabled = scheme.Enabled ?? false,
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
    }

    public async Task<List<AlarmSchemeTypeDto>> GetAlarmSchemeTypes()
    {
        return await db.AlarmSchemeTypes.Select(t => new AlarmSchemeTypeDto
        {
            AlarmSchemeTypeId = t.AlarmSchemeTypeId,
            Key = t.Key,
        }).ToListAsync();
    }

    private AlarmSchemeDto GetAlarmSchemeDto(AlarmScheme scheme)
    {
        return new AlarmSchemeDto
        {
            AlarmSchemeId = scheme.AlarmSchemeId,
            AlarmChannelId = scheme.AlarmChannelId,
            AlarmSchemeTypeId = scheme.AlarmSchemeTypeId,
            Enabled = scheme.Enabled,
            Schedule = new AlarmScheduleDto
            {
                OtherEnabled = scheme.AlarmSchedule?.OtherEnabled ?? false,
                PeopleEnabled = scheme.AlarmSchedule?.PeopleEnabled ?? false,
                PetsEnabled = scheme?.AlarmSchedule?.PetsEnabled ?? false,
                VehicleEnabled = scheme?.AlarmSchedule?.VehicleEnabled ?? false,
            }
        };
    }

    private AlarmScheme GetDefaultScheme(AlarmSchemeDto scheme)
        => new()
        {
            AlarmChannelId = scheme.AlarmChannelId!.Value,
            AlarmSchemeTypeId = scheme.AlarmSchemeTypeId!.Value,
            AlarmSchedule = this.GetDefaultSchedule(),
            DateCreated = DateTime.UtcNow,
            AlarmScheduleId = 0,
            Enabled = false,
        };

    private AlarmSchedule GetDefaultSchedule()
        => new()
        {
            PetsEnabled = false,
            OtherEnabled = false,
            PeopleEnabled = false,
            VehicleEnabled = false,
        };
    
    private async Task<AlarmScheme?> GetScheme(AlarmSchemeDto scheme)
        => (await this.GetChannel(scheme.AlarmChannelId!.Value))
            .AlarmSchemes
            .Where(s => s.AlarmSchemeTypeId == scheme.AlarmSchemeTypeId)
            .OrderByDescending(s => s.DateCreated)
            .FirstOrDefault();
    
    private async Task<AlarmChannel> GetChannel(int channelId)
        => await db.AlarmChannels
            .Include(c => c.AlarmSchemes)
            .ThenInclude(s => s.AlarmSchedule)
            .SingleAsync(c => c.AlarmChannelId == channelId);
}