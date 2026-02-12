#ifndef CAMERA_SETTINGS_CONTROLLER_H
#define CAMERA_SETTINGS_CONTROLLER_H

#include "camera_select_controller.h"

#include <Arduino.h>

typedef void (*CameraSettingsCallback)();

struct CameraSettingsWorkflow {
    CameraSettingsCallback onSuccess;
    CameraSettingsCallback onFailure;
};


void init_camera_settings_controller();

void open_camera_settings_screen(Channel channel, AlarmSchemeEnum alarmScheme);

void monitor_camera_select_network_task();

#endif // CAMERA_SETTINGS_CONTROLLER_H