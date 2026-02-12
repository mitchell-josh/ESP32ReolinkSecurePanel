#include "settings_controller.h"
#include "ui/ui.h"
#include "api/auth_handler.h"
#include "pin_controller.h"
#include "camera_select_controller.h"

#include <Arduino.h>

void open_change_pin();
void modify_full_alarm();
void modify_partial_alarm();
void modify_disarmed_alarm();
void set_ok();
void set_cancel();
bool settings_auth_check();

/**
 * SUB-MENU NAVIGATION
 * These functions redirect the user to the Camera Selection screen
 * while passing the specific Alarm Scheme (Away, Home, or Disarmed).
 */
void modify_full_alarm() {
    if (is_authorised()) {
        open_camera_select_screen(AlarmSchemeEnum::FULL_ALARM);
    }
}

void modify_partial_alarm() {
    if (is_authorised()) {
        open_camera_select_screen(AlarmSchemeEnum::PARTIAL_ALARM);
    }
}

void modify_disarmed_alarm() {
    if (is_authorised()) {
        open_camera_select_screen(AlarmSchemeEnum::DISARMED);
    }
}

/**
 * RETURN LOGIC
 * Transitions back to the PIN screen. It intelligently checks if the 
 * session is still valid to decide whether to show the keypad or the 'Unlocked' status.
 */
void set_ok() {
    open_pin_screen(is_authorised() ? PinMode:: MODE_UNLOCKED : MODE_UNLOCK);
}

void set_cancel() {
    open_pin_screen(is_authorised() ? PinMode::MODE_UNLOCKED : PinMode::MODE_UNLOCK);
}

/**
 * PIN MANAGEMENT
 * Switches the PIN controller to 'Change' mode if the session is still active.
 */
void open_change_pin() {
    if (settings_auth_check()) {
        open_pin_screen(PinMode::MODE_CHANGE_PIN);
    }
}

/**
 * SECURITY CHECK
 * Ensures the user hasn't timed out while looking at the settings menu.
 * If the session has expired, it wipes credentials and forces a login.
 */
bool settings_auth_check() {
    if (is_authorised()) {
        return true;
    }
    else {
        clear_authorised();
        open_pin_screen(PinMode::MODE_UNLOCK);
        return false;
    }
}

/**
 * UI EVENT HANDLER
 * Centralized click listener for all buttons on the Settings Landing page.
 */
static void settings_btn_event_handler(lv_event_t * e) {
    lv_event_code_t code = lv_event_get_code(e);
    lv_obj_t * target = lv_event_get_target(e); // The button object pointer

    if(code == LV_EVENT_CLICKED) {
        if (target == ui_BtnChangePin) open_change_pin();
        else if (target == ui_BtnModifyFull) modify_full_alarm();
        else if (target == ui_BtnModifyPartial) modify_partial_alarm();
        else if (target == ui_BtnModifyDisarmed) modify_disarmed_alarm();
        else if (target == ui_BtnSettingsOkSet) set_ok();
        else if (target == ui_BtnSettingsCancelSet) set_cancel();
    }
}

/**
 * INITIALIZATION
 * Connects the UI buttons defined in LVGL to the event handler logic.
 */
void init_settings_controller() {
    lv_obj_add_event_cb(ui_BtnChangePin, settings_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnModifyFull, settings_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnModifyPartial, settings_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnModifyDisarmed, settings_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnSettingsOkSet, settings_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnSettingsCancelSet, settings_btn_event_handler, LV_EVENT_CLICKED, NULL);
}

/**
 * ENTRY POINT
 * Loads the settings landing screen with a fade animation.
 */
void open_settings_screen() {
    lv_scr_load_anim(ui_SettingsLanding, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, false);
}