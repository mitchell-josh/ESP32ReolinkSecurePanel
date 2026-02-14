#ifndef CAMERA_SELECT_CONTROLLER_H
#define CAMERA_SELECT_CONTROLLER_H

#include "api/api_actions.h"

#include <Arduino.h>
#include <functional>

/**
 * Binds the physical touch events from the LVGL objects to the controller logic.
 * Called once during the device's boot/setup sequence.
 */
void init_camera_select_controller();

/**
 * The primary entry point for this screen.
 * @param alarmScheme The current mode (Away, Home, etc.) to be viewed or edited.
 */
void open_camera_select_screen(AlarmSchemeEnum alarmScheme);

/**
 * The background monitor function.
 * This should be called in the main loop to check if a pending network task
 * has completed, then dispatch the appropriate workflow callback.
 */
void monitor_camera_settings_network_task();

#endif // CAMERA_SELECT_H