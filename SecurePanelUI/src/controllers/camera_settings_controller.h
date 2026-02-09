#ifndef CAMERA_SETTINGS_CONTROLLER_H
#define CAMERA_SETTINGS_CONTROLLER_H

#include "camera_select_controller.h"

#include <Arduino.h>

void init_camera_settings_controller();

void open_camera_settings_screen(Channel channel, AlarmScheme alarmScheme);

#endif // CAMERA_SETTINGS_CONTROLLER_H