#include "pin_controller.h"
#include "settings_controller.h"
#include "api/auth_handler.h"
#include "api/secure_panel_api.h"
#include "ui/ui.h"

#include <Arduino.h>

static PinMode currentMode = PinMode::MODE_UNLOCK;

static String pinBuffer = "";
static String confirmPinBuffer = "";

namespace Texts {
    const char* const ENTER_PIN = "Enter PIN";
    const char* const ENTER_NEW_PIN = "Enter New PIN";
    const char* const CONFIRM_PIN = "Confirm PIN";
    const char* const UNLOCKED = "Unlocked";
}

void set_full_alarm();
void set_partial_alarm();
void open_settings();
void change_pin();
void unlock_pin();
void confirm_change_pin();
bool auth_check();
BooleanResult get_result(const char* jsonString);

void submit_pin() {
    if (pinBuffer.length() != 4) {
        return;
    }

    switch (currentMode) {
        case PinMode::MODE_UNLOCK:
            unlock_pin();
            break;
        case PinMode::MODE_CHANGE_PIN:
            change_pin();
            break;
        case PinMode::MODE_CONFIRM_CHANGE_PIN:
            confirm_change_pin();
            break;
    }
}

void change_pin() {
    Serial.println("Opening Confirm Mode");
    confirmPinBuffer = pinBuffer;
    open_pin_screen(PinMode::MODE_CONFIRM_CHANGE_PIN);
}   

void confirm_change_pin() {
    if (is_authorised()) {
        if (pinBuffer != confirmPinBuffer) {
            open_pin_screen(PinMode::MODE_UNLOCK);
            return;
        }

        AuthCredentials credentials = get_credentials();

        Serial.println(credentials.alarmCode);
        
        RequestModel req;

        // 1. The Base Endpoint (without the ? parameters)
        // We use the macro from your platformio.ini
        req.endpoint = String(SECURE_PANEL_API_URI) + "auth/ChangeAlarmCode";

        // 2. Set Query Parameters (?alarmCode=0000)
        req.query["newAlarmCode"] = pinBuffer;

        // 3. Set Custom Headers
        req.headers["X-Alarm-User"] = credentials.alarmUser;
        req.headers["X-Alarm-Code"] = credentials.alarmCode;

        // 4. Send the Request
        String response = post_data(req);
        
        // 5. Deserialise response
        BooleanResult result = get_result(response.c_str());

        if (result.succeeded) {
            unlock_pin();
        }
    }
}

void unlock_pin() {
    AuthCredentials credentials;
    credentials.alarmCode = pinBuffer;

    authorise(credentials);
    if (is_authorised()) {
        open_pin_screen(PinMode::MODE_UNLOCKED); // open set mode
    }
}

bool auth_check() {
    if (is_authorised()) {
        return true;
    }
    else {
        clear_authorised();
        open_pin_screen(PinMode::MODE_UNLOCK);
        return false;
    }
}

static void pin_btn_event_handler(lv_event_t * e) {
    lv_event_code_t code = lv_event_get_code(e);
    lv_obj_t * target = lv_event_get_target(e); // The button object pointer

    if(code == LV_EVENT_CLICKED) {
        if (target == ui_BtnKeyPad0) pinBuffer += "0";
        else if (target == ui_BtnKeyPad1) pinBuffer += "1";
        else if (target == ui_BtnKeyPad2) pinBuffer += "2";
        else if (target == ui_BtnKeyPad3) pinBuffer += "3";
        else if (target == ui_BtnKeyPad4) pinBuffer += "4";
        else if (target == ui_BtnKeyPad5) pinBuffer += "5";
        else if (target == ui_BtnKeyPad6) pinBuffer += "6";
        else if (target == ui_BtnKeyPad7) pinBuffer += "7";
        else if (target == ui_BtnKeyPad8) pinBuffer += "8";
        else if (target == ui_BtnKeyPad9) pinBuffer += "9";

        else if (target == ui_BtnKeyPadA) set_full_alarm();
        else if (target == ui_BtnKeyPadB) set_partial_alarm();
        else if (target == ui_BtnKeyPadC) open_settings();
        else if (target == ui_BtnKeyPadD) Serial.println("KeyPadD pressed");

        else if (target == ui_BtnKeyPadOk) {
            submit_pin();
        }
        else if (target == ui_BtnKeyPadCancel) {
            pinBuffer = "";
        }

        if (pinBuffer.length() > 4) pinBuffer.remove(4);
        if (pinBuffer.length() > 0) lv_label_set_text(ui_LblKeyPadPrompt, pinBuffer.c_str());
    }
}

void set_full_alarm() {

}

void set_partial_alarm() {

}

void open_settings() {
    if (is_authorised()) {
        open_settings_screen();
    }
}

void update_pin_ui() {
    lv_label_set_text(ui_LblKeyPadPrompt, ""); // Clear text

    switch (currentMode) {
        case PinMode::MODE_UNLOCK:
            lv_label_set_text(ui_LblKeyPadPrompt, Texts::ENTER_PIN);
            break;
        case PinMode::MODE_CHANGE_PIN:
            lv_label_set_text(ui_LblKeyPadPrompt, Texts::ENTER_NEW_PIN);
            break;
        case PinMode::MODE_CONFIRM_CHANGE_PIN:
            lv_label_set_text(ui_LblKeyPadPrompt, Texts::CONFIRM_PIN);
            break;
        case PinMode::MODE_UNLOCKED:
            lv_label_set_text(ui_LblKeyPadPrompt, Texts::UNLOCKED);
            break;
    }
}

void init_pin_controller() {
    lv_obj_add_event_cb(ui_BtnKeyPad0, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPad1, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPad2, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPad3, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPad4, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPad5, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPad6, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPad7, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPad8, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPad9, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);

    lv_obj_add_event_cb(ui_BtnKeyPadA, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPadB, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPadC, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPadD, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);

    lv_obj_add_event_cb(ui_BtnKeyPadOk, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnKeyPadCancel, pin_btn_event_handler, LV_EVENT_CLICKED, NULL);
}

void open_pin_screen(PinMode pinMode) {
    Serial.println(pinMode);
    currentMode = pinMode;
    pinBuffer = "";
    update_pin_ui();
    _ui_screen_change(&ui_PinEntry, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_PinEntry_screen_init);
}