#include "pin_controller.h"
#include "settings_controller.h"
#include "error_controller.h"
#include "api/auth_handler.h"
#include "api/api_actions.h"
#include "api/secure_panel_api.h"
#include "api/network_task.h"
#include "ui/ui.h"

#include <Arduino.h>

PinWorkflow activeWorkflow = { nullptr, nullptr };

static PinMode currentMode = PinMode::MODE_UNLOCK;
static String pinBuffer = "";
static String confirmPinBuffer = "";

extern Auth auth;

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
bool run_auth_check();
void update_pin_ui();
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
    confirmPinBuffer = pinBuffer;
    open_pin_screen(PinMode::MODE_CONFIRM_CHANGE_PIN);
}

void confirm_change_pin() {
    if (!run_auth_check()) {
        Serial.println("Unauthorised - Opening Error Screen");
        open_error_screen("Unauthorised", []() {
            confirmPinBuffer = "";
            open_pin_screen(PinMode::MODE_CONFIRM_CHANGE_PIN);
        });
        return;
    }

    if (pinBuffer != confirmPinBuffer) {
        Serial.println("Pins do not match");
        open_error_screen("PINs do not match", []() {
            confirmPinBuffer = "";
            pinBuffer = "";
            open_pin_screen(PinMode::MODE_CHANGE_PIN);
        });
        return;
    }

    activeWorkflow.onSuccess = []() {
        clear_authorised();
        pinBuffer = "";
        currentMode = PinMode::MODE_UNLOCK;
    };

    activeWorkflow.onFailure = []() {
        open_error_screen("Update Failed", []() {
            pinBuffer = "";
            confirmPinBuffer = "";
            open_pin_screen(PinMode::MODE_CHANGE_PIN);
        });
    };

    // 3. Launch the worker task
    // We pass a wrapper that calls the actual API logic
    run_with_loading([]() {
        // This runs on Core 0
        BooleanResult result = auth.changeAlarmCode(pinBuffer);
        if (!result.succeeded) {
            loadingState = LoadingState::ERROR;
        }
    }, "Updating PIN...");
}

// Static wrappers for callbacks
void on_unlock_success() {
    currentMode = PinMode::MODE_UNLOCKED;
    pinBuffer = "";
}

void on_auth_failure() {
    currentMode = PinMode::MODE_UNLOCK;
    pinBuffer = "";
}

void unlock_pin() {
    activeWorkflow.onSuccess = on_unlock_success;
    activeWorkflow.onFailure = on_auth_failure;

    AuthCredentials credentials;
    credentials.alarmCode = pinBuffer;

    String safeUser = credentials.alarmUser;
    String safeCode = credentials.alarmCode;

    run_with_loading([safeUser, safeCode]() {
        AuthCredentials safeCreds;
        safeCreds.alarmUser = safeUser;
        safeCreds.alarmCode = safeCode;
        BooleanResult result = authorise(safeCreds);
        loadingState = result.succeeded ? LoadingState::SUCCESS : LoadingState::ERROR;
    }, "Unlocking...");
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

        else if (target == ui_BtnKeyPadOk) submit_pin();
        else if (target == ui_BtnKeyPadCancel) pinBuffer = "";

        if (pinBuffer.length() > 4) pinBuffer.remove(4);
        if (pinBuffer.length() > 0) lv_label_set_text(ui_LblKeyPadPrompt, pinBuffer.c_str());
    }
}

void set_full_alarm() {
}

void set_partial_alarm() {
}

void open_settings() {
    if (run_auth_check()) open_settings_screen();
}

bool run_auth_check() {
    PinMode pinMode = PinMode::MODE_UNLOCK;
    if (!is_authorised()) {
        open_error_screen("", [pinMode]() { open_pin_screen(pinMode); });
        return false;
    } 
    return true;
}

void update_pin_ui() {
    if (ui_LblKeyPadPrompt == nullptr) return;
    
    // logic to prevent pinBuffer from overwriting the status
    if (currentMode == PinMode::MODE_UNLOCKED) {
        lv_label_set_text(ui_LblKeyPadPrompt, Texts::UNLOCKED);
        return; 
    }

    if (pinBuffer.length() > 0) {
        lv_label_set_text(ui_LblKeyPadPrompt, pinBuffer.c_str());
    } else {
        switch (currentMode) {
            case PinMode::MODE_UNLOCK: lv_label_set_text(ui_LblKeyPadPrompt, Texts::ENTER_PIN); break;
            case PinMode::MODE_CHANGE_PIN: lv_label_set_text(ui_LblKeyPadPrompt, Texts::ENTER_NEW_PIN); break;
            case PinMode::MODE_CONFIRM_CHANGE_PIN: lv_label_set_text(ui_LblKeyPadPrompt, Texts::CONFIRM_PIN); break;
        }
    }
}

void finish_loading_sequence() {
    // Add a static variable to act as a lock
    static bool is_transitioning = false;
    if (is_transitioning) return;
    is_transitioning = true;

    lv_timer_t * t = lv_timer_create([](lv_timer_t * timer) {   
        if (lv_scr_act() != ui_PinEntry) {
            _ui_screen_change(&ui_PinEntry, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_PinEntry_screen_init);
            update_pin_ui();
        } else {
            update_pin_ui();
        }

        activeWorkflow.onSuccess = nullptr;
        activeWorkflow.onFailure = nullptr;
        
        // Reset the lock
        bool * lock = (bool*)timer->user_data;
        *lock = false;

        lv_timer_del(timer); 
    }, 100, &is_transitioning); // Increased to 100ms for extra safety
}

void monitor_pin_network_task() {
    LoadingState state = loadingState;
    if (state == LoadingState::IDLE || state == LoadingState::LOADING) return;
    
    loadingState = LoadingState::IDLE; // Reset immediately
    delay(50); // Memory sync buffer

    // Use a timer for BOTH Success and Failure to protect UI pointers
    lv_timer_create([](lv_timer_t * t) {
        LoadingState finishedState = (LoadingState)(uintptr_t)t->user_data;

        if (finishedState == LoadingState::SUCCESS) {
            if (activeWorkflow.onSuccess) activeWorkflow.onSuccess();
            finish_loading_sequence(); // Handles transition back to Keypad
        } 
        else {
            pinBuffer = ""; 
            if (activeWorkflow.onFailure) {
                // Capture the callback and null it IMMEDIATELY 
                // to prevent double-execution
                auto failCb = activeWorkflow.onFailure;
                activeWorkflow.onFailure = nullptr; 
                
                failCb(); 
                finish_loading_sequence();
            } else {
                open_error_screen("Connection Error", nullptr);
            }
        }

        activeWorkflow.onSuccess = nullptr;
        activeWorkflow.onFailure = nullptr;
        lv_timer_del(t);
    }, 100, (void*)(uintptr_t)state);
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
    // Safety check: Is the UI actually built?
    if (ui_PinEntry == nullptr) {
        return;
    }

    currentMode = pinMode;
    pinBuffer = "";
    update_pin_ui();
    _ui_screen_change(&ui_PinEntry, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_PinEntry_screen_init);
}