using System.Text.Json.Serialization;
using ReolinkAPI.Shared;

namespace ReolinkAPI.Push;

/// <summary>
/// Represents the configuration values for Push notifications.
/// Contained within the <see cref="PushResponse"/>.
/// </summary>
public record PushValue(
    // Gets or sets the master enable switch for Push notifications. 1: Enabled, 0: Disabled.
    int? Enable,

    /* Gets or sets the detailed AI-aware schedule. This defines which types of detection (Person, Vehicle, etc.)
     trigger pushes at specific times. */
    AiSchedule? Schedule,

    // Gets or sets whether the schedule is currently active.
    int? ScheduleEnabled);