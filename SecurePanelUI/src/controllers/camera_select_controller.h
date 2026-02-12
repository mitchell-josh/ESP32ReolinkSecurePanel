#ifndef CAMERA_SELECT_CONTROLLER_H
#define CAMERA_SELECT_CONTROLLER_H

#include "api/api_actions.h"

#include <Arduino.h>
#include <functional>

typedef std::function<void()> CameraSelectCallback;

struct CameraSelectWorkflow {
    CameraSelectCallback onSuccess;
    CameraSelectCallback onFailure;
};

void init_camera_select_controller();

void open_camera_select_screen(AlarmSchemeEnum alarmScheme);

void monitor_camera_settings_network_task();

#endif // CAMERA_SELECT_H