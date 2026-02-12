#include "error_controller.h"
#include "ui/ui.h"

#include <Arduino.h>

// Storage for the current error text to be displayed
static String errorMessage;

// The function to execute once the user acknowledges the error
static ErrorCallback errorCallback;

/**
 * UI EVENT HANDLER
 * Listens for the 'OK' button click on the error overlay.
 */
void error_btn_event_handler(lv_event_t * e) {
    lv_event_code_t code = lv_event_get_code(e);
    lv_obj_t * target = lv_event_get_target(e); // The button object pointer

    if (code == LV_EVENT_CLICKED) {
        // Execute the recovery logic (e.g., return to PIN screen or retry)
        if (target == ui_BtnErrorOk) errorCallback(); 
    }
}

/**
 * INITIALIZATION
 * Binds the physical touch event of the error button to the handler.
 */
void init_error_controller() {
    lv_obj_add_event_cb(ui_BtnErrorOk, error_btn_event_handler, LV_EVENT_CLICKED, NULL);
}

/**
 * THE ERROR DISPATCHER
 * Transitions the screen to the error view and sets up the recovery path.
 * * @param errorMessage The text describing what went wrong.
 * @param onConfirm A lambda or function pointer to handle the "OK" click.
 */
void open_error_screen(String errorMessage, ErrorCallback onConfirm) {
    lv_scr_load_anim(ui_Error, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, false);

    // Create a static buffer that won't disappear when the function ends
    static char persistentMsg[64]; 
    strncpy(persistentMsg, errorMessage.c_str(), sizeof(persistentMsg) - 1);

    errorCallback = onConfirm;

    lv_timer_create([](lv_timer_t * t) {
        if (ui_LblErrorText != nullptr) {
            // Now we point to the static buffer, not the deleted local String
            lv_label_set_text(ui_LblErrorText, (const char*)t->user_data);
        }
        lv_timer_del(t);
    }, 50, (void*)persistentMsg);
}