#ifndef CAMERA_SETTINGS_CONTROLLER_H
#define CAMERA_SETTINGS_CONTROLLER_H

#include "camera_select_controller.h"

#include <Arduino.h>

/**
 * Function pointer type for UI callbacks.
 * Used to trigger specific UI transitions after a settings 'Save' or 'Load' operation.
 */
typedef void (*CameraSettingsCallback)();

/**
 * Encapsulates the Success and Failure logic for settings operations.
 * This structure allows the UI to react differently depending on whether 
 * the hardware accepted the new configuration.
 */
struct CameraSettingsWorkflow {
    CameraSettingsCallback onSuccess; // Logic to run if settings are successfully saved/loaded
    CameraSettingsCallback onFailure; // Logic to run if a network or validation error occurs
};

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