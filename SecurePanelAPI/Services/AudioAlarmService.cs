using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using ReolinkAPI.Audio;
using ReolinkAPI.BuzzerAlarm;
using ReolinkAPI.Clients;
using ReolinkAPI.Handlers;
using ReolinkAPI.Shared;
using ReolinkAPI.Utils;
using SecurePanelAPI.Utils;
using SecurePanelDb;
using SecurePanelDb.Models;
using SecurePanelModels.DTOs;
using SecurePanelModels.Queries;
using SecurePanelModels.Services;

namespace SecurePanelAPI.Services;

/// <summary>
/// Manages the physical Audio Alarm (Siren) settings on Reolink cameras.
/// Translates database schedules into hardware-specific binary strings.
/// </summary>
public class AudioAlarmService(
    ReolinkClient reolinkClient, 
    SecurePanelDbContext db) : IAudioAlarmService
{
    /// <summary>
    /// Fetches the desired state from the DB and pushes it to the camera API.
    /// </summary>
    public async Task<AlarmResult<bool>> UpdateAudioAlarm(AlarmSchemeQuery query)
    {
        // Get local scheme data
        var scheme = await ApiHttpUtils.GetScheme(db, query);
        if (scheme == null)
        {
            return AlarmResult<bool>.Failure("Scheme not found.");
        }
        
        var raw = await reolinkClient.SetAudioAlarm(GenerateSetAudioRequest(scheme));
        
        var result = ReolinkHandler.ProcessResponse<AudioAlarmResponse>(raw);

        return !result.Succeeded ? AlarmResult<bool>.Failure(result.ErrorMessage!) : AlarmResult<bool>.Success(true);
    }

    /// <summary>
    /// Maps our clean domain model (bools) to the Reolink AI schedule format.
    /// Reolink requires a 168-character string of '1's and '0's for each detection type.
    /// </summary>
    private static SetAudioAlarmRequest GenerateSetAudioRequest(AlarmScheme scheme)
    {
        return new SetAudioAlarmRequest
        {
            Param = new SetAudioAlarmParam
            {
                Audio = new AudioAlarm
                {
                    Enable = scheme.Enabled ? 1 : 0,
                    StopAlarm = 0,
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
}