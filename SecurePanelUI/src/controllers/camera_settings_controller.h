#ifndef CAMERA_SETTINGS_CONTROLLER_H
#define CAMERA_SETTINGS_CONTROLLER_H

#include "camera_select_controller.h"

#include <Arduino.h>

/**
 * Binds the UI elements (dropdowns, save/cancel buttons) to the controller logic.
 * Should be called once during the initial system boot.
 */
void init_camera_settings_controller();

/**
 * The primary entry point for the settings screen.
 * @param channel The specific camera hardware channel to configure.
 * @param alarmScheme The global alarm mode (Home, Away, etc.) currently being modified.
 */
void open_camera_settings_screen(Channel channel, AlarmSchemeEnum alarmScheme);

/**
 * Background monitor for camera selection tasks.
 * Periodically checked by the main loop to handle transitions from the selection screen.
 */
void monitor_camera_select_network_task();

#endif // CAMERA_SETTINGS_CONTROLLER_H