#include "error_controller.h"
#include "ui/ui.h"

#include <Arduino.h>

static String errorMessage;

static ErrorCallback errorCallback;

void error_btn_event_handler(lv_event_t * e) {
    lv_event_code_t code = lv_event_get_code(e);
    lv_obj_t * target = lv_event_get_target(e); // The button object pointer

    if (code == LV_EVENT_CLICKED) {
        if (target == ui_BtnErrorOk) errorCallback(); 
    }
}

void init_error_controller() {
    lv_obj_add_event_cb(ui_BtnErrorOk, error_btn_event_handler, LV_EVENT_CLICKED, NULL);
}

void open_error_screen(String errorMessage, ErrorCallback onConfirm) {
    _ui_screen_change(&ui_Error, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_Error_screen_init);

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