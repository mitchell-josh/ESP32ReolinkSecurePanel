#ifndef API_ACTIONS_H
#define API_ACTIONS_H

#include "auth_handler.h"
#include "secure_panel_api.h"

#include <Arduino.h>

class AlarmScheme {
    public:
        AlarmScheme();

        void getAlarmScheme();
        void saveAlarmScheme();
        void getAlarmSchemeTypes();
};

class AudioAlarm {
    public:
        AudioAlarm();

        void updateAudioAlarm();
};

class Auth {
    public:
        Auth();

        void test();
        BooleanResult changeAlarmCode(String newAlarmCode);
        BooleanResult checkAlarmCode(AuthCredentials credentials);
};

class BuzzerAlarm {
    public:
        BuzzerAlarm();

        void updateBuzzerAlarm();
};

class Channels {
    public:
        Channels();

        void getChannels();
        void createChannels();
        void updateChannels();
};

class Push {
    public:
        Push();

        void updatePush();
};

#endif // API_ACTIONS_H