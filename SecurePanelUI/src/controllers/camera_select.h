#ifndef CAMERA_SELECT_H
#define CAMERA_SELECT_H

#include <Arduino.h>

enum AlarmScheme {
    DISARMED,
    PARTIAL_ALARM,
    FULL_ALARM
};

void init_camera_select_controller();

void open_camera_select_screen(AlarmScheme alarmScheme);

#endif // CAMERA_SELECT_H