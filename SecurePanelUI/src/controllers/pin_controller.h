#ifndef PIN_CONTROLLER_H
#define PIN_CONTROLLER_H

#include <lvgl.h>
#include <functional>

typedef void (*PinCallback)();

struct PinWorkflow {
    PinCallback onSuccess;
    PinCallback onFailure;
};

enum PinMode {
    MODE_UNLOCK,
    MODE_UNLOCKED,
    MODE_CHANGE_PIN,
    MODE_CONFIRM_CHANGE_PIN
};

void init_pin_controller();

void open_pin_screen(PinMode pinMode);

void monitor_pin_network_task();

#endif // PIN_CONTROLLER_H