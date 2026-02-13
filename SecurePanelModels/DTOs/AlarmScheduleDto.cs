using System.Text.Json.Serialization;

namespace SecurePanelModels.DTOs;

/// <summary>
/// A simplified representation of AI detection toggles for use in UI or external APIs.
/// </summary>
public record AlarmScheduleDto(bool? PeopleEnabled, bool? VehicleEnabled, bool? PetsEnabled, bool? OtherEnabled);