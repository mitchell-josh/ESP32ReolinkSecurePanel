#include "camera_select_controller.h"
#include "camera_settings_controller.h"
#include "error_controller.h"
#include "api/auth_handler.h"
#include "api/api_actions.h"
#include "api/secure_panel_api.h"
#include "api/network_task.h"
#include "ui/ui.h"

CameraSelectWorkflow cameraSelectActiveWorkflow = { nullptr, nullptr };

extern Channels channel;

static AlarmSchemeEnum currentScheme = AlarmSchemeEnum::DISARMED;
static Channel currentChannel;

// Initialise array of 8 channels
std::array<Channel, 8> channels = {};

lv_obj_t* cameraLabels[8];
lv_obj_t* cameraButtons[8];

BooleanResult get_result(const char* jsonString);
void open_camera_settings(Channel channel, AlarmSchemeEnum alarmScheme);

void get_channels() {
    channels = channel.getChannels();
}

static void camera_btn_event_handler(lv_event_t * e) {
    lv_event_code_t code = lv_event_get_code(e);
    lv_obj_t * target = lv_event_get_target(e); // The button object pointer

    if(code == LV_EVENT_CLICKED) {
        for (int i = 0; i < 8; i++) {
            if (target == cameraButtons[i]) {
                Channel channel = channels[i];
                if (channel.channelEnabled) {
                    open_camera_settings(channel, currentScheme);
                }
            }
        }
    }
}

void open_camera_settings(Channel channel, AlarmSchemeEnum alarmScheme) {
    currentChannel = channel;

    cameraSelectActiveWorkflow.onSuccess = []() {
        lv_timer_t * t = lv_timer_create([](lv_timer_t * timer) {
            Serial.println("System ready. Transitioning to Settings screen.");
            open_camera_settings_screen(currentChannel, currentScheme);
            lv_timer_del(timer);
      }, 200, NULL); 
    };

    run_with_loading([]() {
        loadingState = LoadingState::SUCCESS;
        delay(50);
    }, "Loading...");
}

void update_camera_select_ui() {
    cameraLabels[0] = ui_LblCamera0;
    cameraLabels[1] = ui_LblCamera1;
    cameraLabels[2] = ui_LblCamera2;
    cameraLabels[3] = ui_LblCamera3;
    cameraLabels[4] = ui_LblCamera4;
    cameraLabels[5] = ui_LblCamera5;
    cameraLabels[6] = ui_LblCamera6;
    cameraLabels[7] = ui_LblCamera7;

    for (int i = 0; i < 8; i++) {
        if (cameraLabels[i] == nullptr) continue;
        Channel channel = channels[i];
        const char* name = channels[i].channelName.c_str();
        if (channel.channelEnabled) {
            lv_label_set_text(cameraLabels[i], name);
        }
        else {
            lv_label_set_text(cameraLabels[i], "");
            lv_obj_add_state(cameraLabels[i], LV_STATE_DISABLED);
        }
    }
}

void finish_camera_select_loading_sequence() {
    // Add a static variable to act as a lock
    static bool is_transitioning = false;
    if (is_transitioning) return;
    is_transitioning = true;

    lv_timer_t * t = lv_timer_create([](lv_timer_t * timer) {   
        update_camera_select_ui();

        cameraSelectActiveWorkflow.onSuccess = nullptr;
        cameraSelectActiveWorkflow.onFailure = nullptr;
        
        // Reset the lock
        bool * lock = (bool*)timer->user_data;
        *lock = false;

        lv_timer_del(timer); 
    }, 100, &is_transitioning); // Increased to 100ms for extra safety
}

void monitor_camera_select_network_task() {
    if (cameraSelectActiveWorkflow.onSuccess == nullptr && cameraSelectActiveWorkflow.onFailure == nullptr) {
        return; 
    }

    LoadingState state = loadingState;
    if (state == LoadingState::IDLE || state == LoadingState::LOADING) return;
    
    loadingState = LoadingState::IDLE; // Reset immediately
    delay(50); // Memory sync buffer

    // Use a timer for BOTH Success and Failure to protect UI pointers
    lv_timer_create([](lv_timer_t * t) {
        LoadingState finishedState = (LoadingState)(uintptr_t)t->user_data;

        if (finishedState == LoadingState::SUCCESS) {
            if (cameraSelectActiveWorkflow.onSuccess) cameraSelectActiveWorkflow.onSuccess();
            finish_camera_select_loading_sequence(); // Handles transition back to Keypad
        } 
        else {
            if (cameraSelectActiveWorkflow.onFailure) {
                // Capture the callback and null it IMMEDIATELY 
                // to prevent double-execution
                auto failCb = cameraSelectActiveWorkflow.onFailure;
                cameraSelectActiveWorkflow.onFailure = nullptr; 
                
                failCb(); 
                finish_camera_select_loading_sequence();
            } else {
                open_error_screen("Connection Error", nullptr);
            }
        }

        cameraSelectActiveWorkflow.onSuccess = nullptr;
        cameraSelectActiveWorkflow.onFailure = nullptr;
        lv_timer_del(t);
    }, 100, (void*)(uintptr_t)state);
}

void init_camera_select_controller() {
    cameraButtons[0] = ui_BtnCamera0;
    cameraButtons[1] = ui_BtnCamera1;
    cameraButtons[2] = ui_BtnCamera2;
    cameraButtons[3] = ui_BtnCamera3;
    cameraButtons[4] = ui_BtnCamera4;
    cameraButtons[5] = ui_BtnCamera5;
    cameraButtons[6] = ui_BtnCamera6;
    cameraButtons[7] = ui_BtnCamera7;

    for (int i = 0; i < 8; i++) {
        lv_obj_add_event_cb(cameraButtons[i], camera_btn_event_handler, LV_EVENT_CLICKED, NULL);
    }
}

void open_camera_select_screen(AlarmSchemeEnum alarmScheme) {
    currentScheme = alarmScheme;
    get_channels();
    update_camera_select_ui();
    lv_scr_load_anim(ui_CameraSelect, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, false);
}