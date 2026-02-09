#ifndef CAMERA_SELECT_CONTROLLER_H
#define CAMERA_SELECT_CONTROLLER_H

#include <Arduino.h>

struct Channel {
    int channelId = -1;
    String channelName = "";
    int channelKey = -1;
    bool channelEnabled = false;
};

enum AlarmScheme {
    DISARMED,
    PARTIAL_ALARM,
    FULL_ALARM
};

void init_camera_select_controller();

void open_camera_select_screen(AlarmScheme alarmScheme);

#endif // CAMERA_SELECT_H