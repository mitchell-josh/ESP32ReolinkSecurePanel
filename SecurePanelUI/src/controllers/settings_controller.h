#ifndef SETTINGS_CONTROLLER_H
#define SETTINGS_CONTROLLER_H

/**
 * Binds the navigation buttons (Change PIN, Modify Modes) to the controller logic.
 * This connects the static UI objects created during the LVGL setup to the 
 * functional code in the implementation file.
 */
void init_settings_controller();

/**
 * The primary entry point for the global configuration menu.
 * Triggers the visual transition to the 'Settings Landing' screen.
 */
void open_settings_screen();

#endif // SETTINGS_CONTROLLER_H