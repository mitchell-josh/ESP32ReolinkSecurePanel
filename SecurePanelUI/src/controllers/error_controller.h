#ifndef ERROR_CONTROLLER_H
#define ERROR_CONTROLLER_H

#include <Arduino.h>
#include <functional>

typedef std::function<void()> ErrorCallback;

void init_error_controller();

void open_error_screen(String message, ErrorCallback errorCallback);

#endif