#include "camera_settings_controller.h"
#include "api/secure_panel_api.h"
#include "api/auth_handler.h"
#include "ui/ui.h"

enum DropdownValues {
    DROPDOWN_ENABLED,
    DROPDOWN_DISABLED
};

static Channel currentChannel;
static AlarmSchemeEnum currentAlarmScheme;
static AlarmSettingsScheme settingsScheme;
static AlarmSchemeType alarmSchemeTypes[3] = {};

AlarmSchemeEnum string_to_alarm_enum(const char* text) {
    if (strcasecmp(text, "Disarmed") == 0) return AlarmSchemeEnum::DISARMED;
    else if (strcasecmp(text, "FullAlarm") == 0) return AlarmSchemeEnum::FULL_ALARM;
    else if (strcasecmp(text, "PartialAlarm") == 0) return AlarmSchemeEnum::PARTIAL_ALARM;
}

AlarmSchemeType get_alarm_scheme_type() {
    for (int i = 0; i < 3; i++) {
        if (string_to_alarm_enum(alarmSchemeTypes[i].key.c_str()) == currentAlarmScheme) {
            return alarmSchemeTypes[i];
        }
    }
    throw;
}

void submit_settings() {
    AlarmSchemeType alarmSchemeType = get_alarm_scheme_type();

    settingsScheme.alarmSchemeTypeId = alarmSchemeType.alarmSchemeTypeId;
    settingsScheme.alarmChannelId = currentChannel.channelId;

    if (is_authorised()) {
        AuthCredentials credentials = get_credentials();

        RequestModel req;

        req.endpoint = String(SECURE_PANEL_API_URI) + "alarmscheme/SaveAlarmScheme";

        // 3. Set Custom Headers
        req.headers["X-Alarm-User"] = credentials.alarmUser;
        req.headers["X-Alarm-Code"] = credentials.alarmCode;

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

        req.body = doc;

        String response = post_data(req);
    }
}

void update_current_alarm_scheme() {
    if (is_authorised()) {
        AuthCredentials credentials = get_credentials();
        {
            AlarmSchemeType alarmSchemeType = get_alarm_scheme_type();

            RequestModel req;

            // 1. The Base Endpoint (without the ? parameters)
            // We use the macro from your platformio.ini
            req.endpoint = String(SECURE_PANEL_API_URI) + "alarmscheme/GetAlarmScheme";

            // 3. Set Custom Headers
            req.headers["X-Alarm-User"] = credentials.alarmUser;
            req.headers["X-Alarm-Code"] = credentials.alarmCode;

            Serial.println("Getting alarm scheme");

            // 4. Set Custom body
            JsonDocument body;
            body["ChannelId"] = currentChannel.channelId;
            body["AlarmSchemeTypeId"] = alarmSchemeType.alarmSchemeTypeId;

            req.body = body;

            // 5. Send the request
            String response = post_data(req);

            Serial.println(response);

            JsonDocument doc;
            DeserializationError error = deserializeJson(doc, response);

            if (error) {
                Serial.print("JSON Parse failed: ");
                Serial.println(error.c_str());
                return;
            }

            if (doc["succeeded"] == true) {
                settingsScheme.alarmSchemeId = doc["value"]["alarmSchemeId"];
                settingsScheme.alarmChannelId = doc["value"]["alarmChannelId"];
                settingsScheme.alarmSchemeTypeId = doc["value"]["alarmSchemeTypeId"];
                settingsScheme.enabled = doc["value"]["enabled"] | false;
                settingsScheme.pushEnabled = doc["value"]["pushEnabled"] | false;
                settingsScheme.schedule.otherEnabled = doc["value"]["schedule"]["otherEnabled"] | false;
                settingsScheme.schedule.peopleEnabled = doc["value"]["schedule"]["peopleEnabled"] | false;
                settingsScheme.schedule.petsEnabled = doc["value"]["schedule"]["petsEnabled"] | false;
                settingsScheme.schedule.vehicleEnabled = doc["value"]["schedule"]["vehicleEnabled"] | false;
            }
        }
    }
}

static void update_alarm_scheme_types() {
    if (is_authorised()) {
        AuthCredentials credentials = get_credentials(); 
        {
            RequestModel req;

            // 1. The Base Endpoint (without the ? parameters)
            // We use the macro from your platformio.ini
            req.endpoint = String(SECURE_PANEL_API_URI) + "alarmscheme/getalarmschemetypes";

            // 3. Set Custom Headers
            req.headers["X-Alarm-User"] = credentials.alarmUser;
            req.headers["X-Alarm-Code"] = credentials.alarmCode;

            // 4. Send the Request
            String response = get_data(req);

            Serial.println(response);

            JsonDocument doc;
            DeserializationError error = deserializeJson(doc, response);

            if (error) {
                Serial.print("JSON Parse failed: ");
                Serial.println(error.c_str());
                return;
            }
            
            if (doc["succeeded"] == true) {
                JsonArray arr = doc["value"].as<JsonArray>();
                
                // 3. Check if the array actually exists in the JSON
                if (arr.isNull()) {
                    Serial.println("Error: 'value' array is null in JSON");
                    return; 
                }
                int count = 0;
                for (JsonObject obj : arr) {
                    if (count < 3) {
                        alarmSchemeTypes[count].alarmSchemeTypeId = obj["alarmSchemeTypeId"] | -1;
                        alarmSchemeTypes[count].key = obj["key"] | "";
                        count++;
                    }
                }
            }
        }
    }
}

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
        else if (target == ui_BtnSettingsCancelCS) open_camera_select_screen(currentAlarmScheme);
    }
}

void update_camera_settings_ui() {
    if (currentChannel.channelName.length() > 0) {
        lv_label_set_text(ui_LblCameraName, currentChannel.channelName.c_str());
    }

    lv_dropdown_set_selected(ui_DropdownEnabled, settingsScheme.enabled ? 0 : 1);
    lv_dropdown_set_selected(ui_DropdownCarsEnabled, settingsScheme.schedule.vehicleEnabled ? 0 : 1);
    lv_dropdown_set_selected(ui_DropdownOtherEnabled, settingsScheme.schedule.otherEnabled ? 0 : 1);
    lv_dropdown_set_selected(ui_DropdownPeopleEnabled, settingsScheme.schedule.peopleEnabled ? 0 : 1);
    lv_dropdown_set_selected(ui_DropdownPetsEnabled, settingsScheme.schedule.petsEnabled ? 0 : 1);
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

void open_camera_settings_screen(Channel channel, AlarmSchemeEnum alarmScheme) {
    currentChannel = channel;
    currentAlarmScheme = alarmScheme;
    update_alarm_scheme_types();
    update_current_alarm_scheme();
    update_camera_settings_ui();
    _ui_screen_change(&ui_CameraSettings, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_CameraSettings_screen_init);
}