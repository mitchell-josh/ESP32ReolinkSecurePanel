#include "camera_settings_controller.h"
#include "camera_select_controller.h"
#include "error_controller.h"
#include "pin_controller.h"
#include "ui_workflows.h"
#include "api/auth_handler.h"
#include "api/api_actions.h"
#include "api/secure_panel_api.h"
#include "api/network_task.h"
#include "ui/ui.h"

#include <array>

/**
 * Global workflow tracker for async Save/Load operations.
 */
extern UIWorkflow cameraSettingsWorkflow;

/**
 * Dropdown Index Mapping
 * Matches the order of items in the UI (e.g., 0: Enabled, 1: Disabled).
 */
enum DropdownValues {
    DROPDOWN_ENABLED,
    DROPDOWN_DISABLED
};

// Internal state for the current editing context
static Channel currentChannel;
static AlarmSchemeEnum currentAlarmScheme;
static AlarmSettingsScheme settingsScheme;

extern AlarmSchemeController alarmSchemeController;

void go_back();

/**
 * PERSISTENCE: SUBMIT SETTINGS
 * Triggers the background network task to save current settingsScheme to the DB.
 */
void submit_settings() {
    cameraSettingsWorkflow.onSuccess = []() {
        lv_timer_t * t = lv_timer_create([](lv_timer_t * timer) {
            open_camera_select_screen(currentAlarmScheme);
            lv_timer_del(timer);
        }, 200, NULL);
    };

    cameraSettingsWorkflow.onFailure = []() {
        lv_timer_create([](lv_timer_t * t) {
            open_error_screen("Update Failed...", []() {
                lv_timer_create([](lv_timer_t * retryTimer) {
                    open_camera_settings_screen(currentChannel, currentAlarmScheme);
                    lv_timer_del(retryTimer);
                }, 50, NULL);
            });
            lv_timer_del(t);
        }, 200, NULL);
    };

    run_with_loading([]() {
        BooleanResult result = alarmSchemeController.saveAlarmScheme(settingsScheme);
        if (!result.succeeded) {
            loadingState = LoadingState::ERROR;
            delay(50);
        }
    }, "Saving Alarm Settings...");
}

void update_current_alarm_scheme() {
    settingsScheme = alarmSchemeController.getAlarmScheme(
        currentChannel.channelId, 
        currentAlarmScheme);
}

/**
 * DATA MAPPING: PARSE DROPDOWN
 * Helper to convert LVGL dropdown selection index to a boolean.
 */
bool parse_dropdown(lv_event_t * e) {
    lv_obj_t * dropdown = lv_event_get_target(e);

    uint16_t selected = lv_dropdown_get_selected(dropdown);

    if (selected == DropdownValues::DROPDOWN_ENABLED) {
        return true;
    }
    else if (selected == DropdownValues::DROPDOWN_DISABLED) {
        return false;
    }

    return false; // default to false
}

/**
 * UI EVENT HANDLER
 * Updates the local 'settingsScheme' struct in real-time as the user changes dropdowns.
 */
static void camera_settings_btn_event_handler(lv_event_t * e) {
    lv_event_code_t code = lv_event_get_code(e);
    lv_obj_t * dropdown = lv_event_get_target(e);
    lv_obj_t * target = lv_event_get_target(e);

    if (code == LV_EVENT_VALUE_CHANGED) {
        if (dropdown == ui_DropdownEnabled) settingsScheme.enabled = parse_dropdown(e);
        else if (dropdown == ui_DropdownEnabled) settingsScheme.pushEnabled = parse_dropdown(e);
        else if (dropdown == ui_DropdownCarsEnabled) settingsScheme.schedule.vehicleEnabled = parse_dropdown(e);
        else if (dropdown == ui_DropdownOtherEnabled) settingsScheme.schedule.otherEnabled = parse_dropdown(e);
        else if (dropdown == ui_DropdownPeopleEnabled) settingsScheme.schedule.peopleEnabled = parse_dropdown(e);
        else if (dropdown == ui_DropdownPetsEnabled) settingsScheme.schedule.petsEnabled = parse_dropdown(e);
    }

    if (code == LV_EVENT_CLICKED) {
        if (target == ui_BtnSettingsOkCS) submit_settings();
        else if (target == ui_BtnSettingsCancelCS) go_back();
    }
}

void go_back() {
    cameraSettingsWorkflow.onSuccess = []() {
        lv_timer_t * t = lv_timer_create([](lv_timer_t * timer) {
            open_camera_select_screen(currentAlarmScheme);
            lv_timer_del(timer);
        }, 200, NULL);
    };

    cameraSettingsWorkflow.onFailure = []() {
        lv_timer_create([](lv_timer_t * t) {
            open_error_screen("Failed to return. Retrying.", []() {
                lv_timer_create([](lv_timer_t * retryTimer) {
                    clear_authorised();
                    open_pin_screen(PinMode::MODE_UNLOCK);
                    lv_timer_del(retryTimer);
                }, 50, NULL);
            });
            lv_timer_del(t);
        }, 200, NULL);
    };

    run_with_loading([]() {
        loadingState = LoadingState::SUCCESS;
        delay(50);
    }, "Going Back...");
}

/**
 * UI SYNC: REFRESH DROPDOWNS
 * Pulls data from 'settingsScheme' and sets the visual state of the UI components.
 */
void update_camera_settings_ui() {
    if (ui_LblCameraName == nullptr) return;
    if (ui_DropdownEnabled == nullptr) return;
    if (ui_DropdownCarsEnabled == nullptr) return;
    if (ui_DropdownOtherEnabled == nullptr) return;
    if (ui_DropdownPeopleEnabled == nullptr) return;
    if (ui_DropdownPetsEnabled == nullptr) return;

    if (currentChannel.channelName.length() > 0) {
        lv_label_set_text(ui_LblCameraName, currentChannel.channelName.c_str());
    }

    JsonDocument doc;
    doc["alarmSchemeId"] = settingsScheme.alarmSchemeId;
    doc["alarmChannelId"] = settingsScheme.alarmChannelId;
    doc["alarmSchemeTypeId"] = settingsScheme.alarmSchemeTypeId;
    doc["enabled"] = settingsScheme.enabled;
    doc["pushEnabled"] = settingsScheme.pushEnabled;
    doc["schedule"]["otherEnabled"] = settingsScheme.schedule.otherEnabled;
    doc["schedule"]["peopleEnabled"] = settingsScheme.schedule.peopleEnabled;
    doc["schedule"]["petsEnabled"] = settingsScheme.schedule.petsEnabled;
    doc["schedule"]["vehicleEnabled"] = settingsScheme.schedule.vehicleEnabled;        

    String output;
    serializeJson(doc, output);

    lv_dropdown_set_selected(ui_DropdownEnabled, settingsScheme.enabled ? 0 : 1);
    lv_dropdown_set_selected(ui_DropdownCarsEnabled, settingsScheme.schedule.vehicleEnabled ? 0 : 1);
    lv_dropdown_set_selected(ui_DropdownOtherEnabled, settingsScheme.schedule.otherEnabled ? 0 : 1);
    lv_dropdown_set_selected(ui_DropdownPeopleEnabled, settingsScheme.schedule.peopleEnabled ? 0 : 1);
    lv_dropdown_set_selected(ui_DropdownPetsEnabled, settingsScheme.schedule.petsEnabled ? 0 : 1);
}

void finish_camera_settings_loading_sequence() {
    // Add a static variable to act as a lock
    static bool is_transitioning = false;
    if (is_transitioning) return;
    is_transitioning = true;

    lv_timer_t * t = lv_timer_create([](lv_timer_t * timer) {   
        if (lv_scr_act() != ui_CameraSettings) {
            update_camera_settings_ui();
        } else {
            update_camera_settings_ui();
        }

        cameraSettingsWorkflow.clear();
        
        // Reset the lock
        bool * lock = (bool*)timer->user_data;
        *lock = false;

        lv_timer_del(timer); 
    }, 100, &is_transitioning); // Increased to 100ms for extra safety
}

/**
 * ASYNC MONITOR
 * Checks the status of the background core (Core 0) and executes UI callbacks on Core 1.
 */
void monitor_camera_settings_network_task() {
    if (cameraSettingsWorkflow.onSuccess == nullptr && cameraSettingsWorkflow.onFailure == nullptr) {
        return; 
    }

    LoadingState state = loadingState;
    if (state == LoadingState::IDLE || state == LoadingState::LOADING) return;
    
    loadingState = LoadingState::IDLE; // Reset immediately
    delay(100); // Memory sync buffer

    // Use a timer for BOTH Success and Failure to protect UI pointers
    lv_timer_create([](lv_timer_t * t) {
        LoadingState finishedState = (LoadingState)(uintptr_t)t->user_data;

        if (finishedState == LoadingState::SUCCESS) {
            if (cameraSettingsWorkflow.onSuccess) cameraSettingsWorkflow.onSuccess();
            finish_camera_settings_loading_sequence();
        } 
        else {
            if (cameraSettingsWorkflow.onFailure) {
                // Capture the callback and null it IMMEDIATELY 
                // to prevent double-execution
                auto failCb = cameraSettingsWorkflow.onFailure;
                cameraSettingsWorkflow.onFailure = nullptr; 
                
                failCb(); 
                finish_camera_settings_loading_sequence();
            } else {
                open_error_screen("Connection Error", nullptr);
            }
        }

        cameraSettingsWorkflow.onSuccess = nullptr;
        cameraSettingsWorkflow.onFailure = nullptr;
        lv_timer_del(t);
    }, 100, (void*)(uintptr_t)state);
}

void init_camera_settings_controller() {
    lv_obj_add_event_cb(ui_DropdownEnabled, camera_settings_btn_event_handler, LV_EVENT_VALUE_CHANGED, NULL);
    lv_obj_add_event_cb(ui_DropdownPeopleEnabled, camera_settings_btn_event_handler, LV_EVENT_VALUE_CHANGED, NULL);
    lv_obj_add_event_cb(ui_DropdownCarsEnabled, camera_settings_btn_event_handler, LV_EVENT_VALUE_CHANGED, NULL);
    lv_obj_add_event_cb(ui_DropdownPetsEnabled, camera_settings_btn_event_handler, LV_EVENT_VALUE_CHANGED, NULL);
    lv_obj_add_event_cb(ui_DropdownOtherEnabled, camera_settings_btn_event_handler, LV_EVENT_VALUE_CHANGED, NULL);
    lv_obj_add_event_cb(ui_BtnSettingsOkCS, camera_settings_btn_event_handler, LV_EVENT_CLICKED, NULL);
    lv_obj_add_event_cb(ui_BtnSettingsCancelCS, camera_settings_btn_event_handler, LV_EVENT_CLICKED, NULL);
}

/**
 * ENTRY POINT
 * Fetches the latest settings from the server for this camera/mode and loads the screen.
 */
void open_camera_settings_screen(Channel channel, AlarmSchemeEnum alarmScheme) {
    currentChannel = channel;
    currentAlarmScheme = alarmScheme;
    update_current_alarm_scheme();
    update_camera_settings_ui();
    lv_scr_load_anim(ui_CameraSettings, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, false);
}