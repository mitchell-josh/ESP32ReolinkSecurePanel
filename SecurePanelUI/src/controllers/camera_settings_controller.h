#ifndef CAMERA_SETTINGS_CONTROLLER_H
#define CAMERA_SETTINGS_CONTROLLER_H

#include "camera_select_controller.h"

#include <Arduino.h>

struct AlarmSchemeType {
    int alarmSchemeTypeId;
    String key;
};

struct AlarmSettingsSchedule {
    bool peopleEnabled;
    bool vehicleEnabled;
    bool petsEnabled;
    bool otherEnabled;
};

struct AlarmSettingsScheme {
    int alarmSchemeId;
    int alarmChannelId;
    int alarmSchemeTypeId;
    bool enabled;
    bool pushEnabled;
    AlarmSettingsSchedule schedule;
};

void init_camera_settings_controller();

void open_camera_settings_screen(Channel channel, AlarmSchemeEnum alarmScheme);

#endif // CAMERA_SETTINGS_CONTROLLER_H