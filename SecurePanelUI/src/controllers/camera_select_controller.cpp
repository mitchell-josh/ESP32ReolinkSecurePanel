#include "camera_select_controller.h"
#include "api/auth_handler.h"
#include "api/secure_panel_api.h"
#include "ui/ui.h"

struct Channel {
    int channelId = -1;
    String channelName = "";
    int channelKey = -1;
    bool channelEnabled = false;
};

static AlarmScheme currentScheme = AlarmScheme::DISARMED;

// Initialise array of 8 channels
Channel channels[8] = {};

lv_obj_t* cameraLabels[8];

BooleanResult get_result(const char* jsonString);

bool create_channels() {
    if (is_authorised()) {
        AuthCredentials credentials = get_credentials();

        RequestModel req;

        req.endpoint = String(SECURE_PANEL_API_URI) + "channels/createchannels";

        req.headers["X-Alarm-User"] = credentials.alarmUser;
        req.headers["X-Alarm-Code"] = credentials.alarmCode;

        String response = post_data(req);
        
        // 5. Deserialise response
        BooleanResult result = get_result(response.c_str());

        return result.succeeded;
    }
    return false;
}

bool update_channels() {
    if (is_authorised()) {
        AuthCredentials credentials = get_credentials();

        RequestModel req;

        req.endpoint = String(SECURE_PANEL_API_URI) + "channels/updatechannels";

        req.headers["X-Alarm-User"] = credentials.alarmUser;
        req.headers["X-Alarm-Code"] = credentials.alarmCode;

        String response = post_data(req);
        
        // 5. Deserialise response
        BooleanResult result = get_result(response.c_str());

        return result.succeeded;
    }
    return false;
}

void get_channels() {
 if (is_authorised() && create_channels() && update_channels()) {
        AuthCredentials credentials = get_credentials();
        
        RequestModel req;

        // 1. The Base Endpoint (without the ? parameters)
        // We use the macro from your platformio.ini
        req.endpoint = String(SECURE_PANEL_API_URI) + "channels/getchannels";

        // 3. Set Custom Headers
        req.headers["X-Alarm-User"] = credentials.alarmUser;
        req.headers["X-Alarm-Code"] = credentials.alarmCode;

        // 4. Send the Request
        String response = get_data(req);

        JsonDocument doc;
        DeserializationError error = deserializeJson(doc, response);

        if (error) {
            Serial.print("JSON Parse failed: ");
            Serial.println(error.c_str());
            return;
        }
        
        if (doc["succeeded"] == true) {
            Serial.println("response succeeded...");
            JsonArray arr = doc["value"].as<JsonArray>();
            
            // 3. Check if the array actually exists in the JSON
            if (arr.isNull()) {
                Serial.println("Error: 'value' array is null in JSON");
                return; 
            }
            int count = 0;
            for (JsonObject obj : arr) {
                if (count < 8) {
                    channels[count].channelId = obj["channelId"] | -1;
                    channels[count].channelKey = obj["channelKey"] | -1;
                    channels[count].channelEnabled = obj["channelEnabled"] | false;
                    channels[count].channelName = obj["channelName"] | "";
                    count++;
                }
            }
        }
    }
}

static void camera_btn_event_handler(lv_event_t * e) {
    lv_event_code_t code = lv_event_get_code(e);
    lv_obj_t * target = lv_event_get_target(e); // The button object pointer

    if(code == LV_EVENT_CLICKED) {
        for (int i = 0; i < 8; i++) {
            if (target == cameraLabels[i]) {
                Serial.println("Camera selected");
            }
        }
    }
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
        if (strlen(name) > 0) {
            lv_label_set_text(cameraLabels[i], name);
        }
        else {
            lv_label_set_text(cameraLabels[i], "");
            lv_obj_add_state(cameraLabels[i], LV_STATE_DISABLED);
        }
    }
}

void init_camera_select_controller() {
}

void open_camera_select_screen(AlarmScheme alarmScheme) {
    currentScheme = alarmScheme;
    get_channels();
    update_camera_select_ui();
    _ui_screen_change(&ui_CameraSelect, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_CameraSelect_screen_init);
}