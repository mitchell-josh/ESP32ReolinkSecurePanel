#ifndef API_ACTIONS_H
#define API_ACTIONS_H

#include "auth_handler.h"
#include "secure_panel_api.h"

#include <Arduino.h>
#include <array>

/**
 * @brief Represents the detection schedule for a specific alarm scheme.
 */
struct AlarmSettingsSchedule {
    bool peopleEnabled;
    bool vehicleEnabled;
    bool petsEnabled;
    bool otherEnabled;
};

/**
 * @brief Full configuration for a camera's alarm behavior.
 */
struct AlarmSettingsScheme {
    int alarmSchemeId;
    int alarmChannelId;
    int alarmSchemeTypeId;
    bool enabled;
    bool pushEnabled;
    AlarmSettingsSchedule schedule;
};

/**
 * @brief Data structure for physical camera channels.
 */
struct Channel {
    int channelId = -1;
    String channelName = "";
    int channelKey = -1;
    bool channelEnabled = false;
};

/**
 * @brief Available system states for the alarm.
 */
enum AlarmSchemeEnum {
    DISARMED,
    PARTIAL_ALARM,
    FULL_ALARM
};

// -----------------------------------------------------------------------------
// API CONTROLLERS
// -----------------------------------------------------------------------------

class AlarmSchemeController {
    public:
        AlarmSchemeController();

        /**
         * @brief Fetches current settings for a specific camera and mode.
         * @param channelId The ID of the camera channel.
         * @param alarmScheme The mode (Disarmed, Partial, Full).
         */
        AlarmSettingsScheme getAlarmScheme(int channelId, AlarmSchemeEnum alarmScheme);

        /**
         * @brief Saves modified settings back to the server.
         */
        BooleanResult saveAlarmScheme(AlarmSettingsScheme settingsScheme);
        
        /**
         * @brief Globally sets the alarm state (Arm/Disarm).
         */
        BooleanResult setAlarm(AlarmSchemeEnum alarmSchemeType);
};


class AuthController {
    public:
        AuthController();

        /**
         * @brief API test call.
         */
        BooleanResult test();

        /**
         * @brief Modify the alarm code.
         * @param newAlarmCode The new alarm code.
         */
        BooleanResult changeAlarmCode(String newAlarmCode);

        /**
         * @brief Change the alarm code.
         * @param credentials The username and password of the currently logged in user.
         */
        BooleanResult checkAlarmCode(AuthCredentials credentials);
};

class ChannelController {
    public:
        ChannelController();

        /**
         * @brief Retrieves the list of all available camera channels.
         * @return std::array of 8 Channel objects.
         */
        std::array<Channel, 8> getChannels();
};

#endif // API_ACTIONS_H