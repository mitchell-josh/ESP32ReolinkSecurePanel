#ifndef API_ACTIONS_H
#define API_ACTIONS_H

#include "auth_handler.h"
#include "secure_panel_api.h"

#include <Arduino.h>
#include <array>

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

struct Channel {
    int channelId = -1;
    String channelName = "";
    int channelKey = -1;
    bool channelEnabled = false;
};

enum AlarmSchemeEnum {
    DISARMED,
    PARTIAL_ALARM,
    FULL_ALARM
};

class AlarmScheme {
    public:
        AlarmScheme();

        AlarmSettingsScheme getAlarmScheme(int channelId, AlarmSchemeEnum alarmScheme);
        BooleanResult saveAlarmScheme(AlarmSettingsScheme settingsScheme);
        BooleanResult setAlarm(AlarmSchemeEnum alarmSchemeType);
};


class Auth {
    public:
        Auth();

        BooleanResult test();
        BooleanResult changeAlarmCode(String newAlarmCode);
        BooleanResult checkAlarmCode(AuthCredentials credentials);
};

class Channels {
    public:
        Channels();

        std::array<Channel, 8> getChannels();
};

#endif // API_ACTIONS_H