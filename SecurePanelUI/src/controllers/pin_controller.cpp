#include "pin_controller.h"
#include "ui/ui.h"

#include <Arduino.h>

static PinMode currentMode = PinMode::MODE_UNLOCK;

static String pinBuffer = "";

namespace Texts {
    const char* const ENTER_PIN = "Enter PIN";
    const char* const ENTER_NEW_PIN = "Enter New PIN";
    const char* const CONFIRM_PIN = "Confirm PIN";
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

        else if (target == ui_BtnKeyPadA) Serial.println("KeyPadA pressed");
        else if (target == ui_BtnKeyPadB) Serial.println("KeyPadB pressed");
        else if (target == ui_BtnKeyPadC) Serial.println("KeyPadC pressed");
        else if (target == ui_BtnKeyPadD) Serial.println("KeyPadD pressed");

        else if (target == ui_BtnKeyPadOk) {
            pinBuffer = ""; // Clear after submit
        }
        else if (target == ui_BtnKeyPadCancel) {
            pinBuffer = "";
        }

        if (pinBuffer.length() > 4) pinBuffer.remove(4);

        lv_label_set_text(ui_LblKeyPadPrompt, pinBuffer.c_str());
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
    currentMode = pinMode;
    pinBuffer = "";
    update_pin_ui();
    _ui_screen_change(&ui_PinEntry, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_PinEntry_screen_init);
}