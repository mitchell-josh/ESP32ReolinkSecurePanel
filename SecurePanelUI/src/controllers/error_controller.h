#ifndef ERROR_CONTROLLER_H
#define ERROR_CONTROLLER_H

#include <Arduino.h>
#include <functional>

/**
 * Type definition for the error recovery logic.
 * This allows the UI to pass a lambda or function that defines 
 * what happens after the user acknowledges the error (e.g., "Return to Home").
 */
typedef std::function<void()> ErrorCallback;

/**
 * Binds the 'OK' button on the error screen to the internal event handler.
 * This should be called during the global setup of the device.
 */
void init_error_controller();

/**
 * Triggers a visual transition to the Error Screen.
 * * @param message The specific error text to display to the user.
 * @param errorCallback The logic to execute when the user clicks 'OK'.
 */
void open_error_screen(String message, ErrorCallback errorCallback);

#endif