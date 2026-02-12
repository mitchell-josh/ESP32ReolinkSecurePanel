#include "api_actions.h"
#include "api/auth_handler.h"
#include "api/secure_panel_api.h"

#include <Arduino.h>

// Global instances for controlling the system via the UI or other hardware triggers
AuthController authController{};
AlarmSchemeController alarmSchemeController{};
ChannelController channelController{};

// Default constructors
AuthController::AuthController() {}
AlarmSchemeController::AlarmSchemeController() {}
ChannelController::ChannelController() {}

// Forward declaration for helper to parse BooleanResult from JSON strings
BooleanResult get_result(const char* jsonString);

/**
 * AUTHENTICATION CONTROLLER
 * Manages identity validation and credential updates.
 */

// Simple connectivity check to verify API accessibility
BooleanResult AuthController::test() {
    RequestModel request;
    request.endpoint = String(SECURE_PANEL_API_URI) + "auth/test";
    String response = get_data(request);
    return get_result(response.c_str());
}

// Validates a user PIN against the backend; sends credentials in both query and custom security headers
BooleanResult AuthController::checkAlarmCode(AuthCredentials credentials) {
    RequestModel request;
    request.endpoint = String(SECURE_PANEL_API_URI) + "auth/CheckAlarmCode";
    request.query["alarmCode"] = credentials.alarmCode;
    request.headers["X-Alarm-User"] = credentials.alarmUser;
    request.headers["X-Alarm-Code"] = credentials.alarmCode;
    String response = post_data(request);
    return get_result(response.c_str());
}

// Updates the current user's PIN on the server after retrieving current local credentials
BooleanResult AuthController::changeAlarmCode(String newAlarmCode) {
    AuthCredentials credentials = get_credentials();
    RequestModel request;
    request.endpoint = String(SECURE_PANEL_API_URI) + "auth/ChangeAlarmCode";
    request.query["newAlarmCode"] = newAlarmCode;
    request.headers["X-Alarm-User"] = credentials.alarmUser;
    request.headers["X-Alarm-Code"] = credentials.alarmCode;
    String response = post_data(request);
    return get_result(response.c_str());
}

/**
 * ALARM SCHEME CONTROLLER
 * Manages the logic for different security modes (Home, Away, etc.) and their schedules.
 */

// Fetches the specific detection settings (People, Pets, Vehicles) for a given channel and mode
AlarmSettingsScheme AlarmSchemeController::getAlarmScheme(int channelId, AlarmSchemeEnum alarmScheme) {
    AuthCredentials credentials = get_credentials();
    RequestModel request;
    request.endpoint = String(SECURE_PANEL_API_URI) + "alarmscheme/GetAlarmScheme";
    request.headers["X-Alarm-User"] = credentials.alarmUser;
    request.headers["X-Alarm-Code"] = credentials.alarmCode;

    JsonDocument body;
    body["ChannelId"] = channelId;
    body["AlarmSchemeType"] = alarmScheme;
    request.body = body;

    String response = post_data(request);

    AlarmSettingsScheme settingsScheme;

    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, response);
    if (error) {
        Serial.print("JSON Parse failed: ");
        Serial.println(error.c_str());
        return settingsScheme;
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
    return settingsScheme;
}

// Persists local scheme settings to the backend database
BooleanResult AlarmSchemeController::saveAlarmScheme(AlarmSettingsScheme settingsScheme) {
    AuthCredentials credentials = get_credentials();
    RequestModel req;
    req.endpoint = String(SECURE_PANEL_API_URI) + "alarmscheme/SaveAlarmScheme";
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

    BooleanResult result;
    result.succeeded = false;
    result.value = false;

    String response = post_data(req);
    JsonDocument resultDoc;
    DeserializationError error = deserializeJson(resultDoc, response);
    if (error) {
        Serial.print("JSON Parse failed: ");
        Serial.println(error.c_str());
        return result;
    }

    return get_result(response.c_str());
}

// Triggers a global state change (e.g., set whole system to 'Full Arm')
BooleanResult AlarmSchemeController::setAlarm(AlarmSchemeEnum alarmSchemeType) {
    AuthCredentials credentials = get_credentials();
    RequestModel req;
    req.endpoint = String(SECURE_PANEL_API_URI) + "alarmscheme/SetAlarm";
    req.query["alarmSchemeType"] = alarmSchemeType;
    req.headers["X-Alarm-User"] = credentials.alarmUser;
    req.headers["X-Alarm-Code"] = credentials.alarmCode;

    BooleanResult result;
    result.succeeded = false;
    result.value = false;

    String response = post_data(req);
    JsonDocument resultDoc;
    DeserializationError error = deserializeJson(resultDoc, response);
    if (error) {
        Serial.print("JSON Parse failed: ");
        Serial.println(error.c_str());
        return result;
    }

    return get_result(response.c_str());
}

/**
 * CHANNEL CONTROLLER
 * Manages the list of hardware camera channels.
 */

// Retrieves an array of up to 8 camera channels from the system
std::array<Channel, 8> ChannelController::getChannels() {
    AuthCredentials credentials = get_credentials(); 
    RequestModel request;
    request.endpoint = String(SECURE_PANEL_API_URI) + "channels/GetChannels";
    request.headers["X-Alarm-User"] = credentials.alarmUser;
    request.headers["X-Alarm-Code"] = credentials.alarmCode;

    // 4. Send the Request
    String response = get_data(request);
    std::array<Channel, 8> results;

    JsonDocument doc;
    DeserializationError error = deserializeJson(doc, response);
    if (error) {
        Serial.print("JSON Parse failed: ");
        Serial.println(error.c_str());
        return results;
    }
    
    if (doc["succeeded"] == true) {
        JsonArray arr = doc["value"].as<JsonArray>();
        
        // 3. Check if the array actually exists in the JSON
        if (arr.isNull()) {
            Serial.println("Error: 'value' array is null in JSON");
            return results;
        }
        int count = 0;
        for (JsonObject obj : arr) {
            if (count < 8) {
                results[count].channelId = obj["channelId"] | -1;
                results[count].channelKey = obj["channelKey"] | -1;
                results[count].channelEnabled = obj["channelEnabled"] | false;
                results[count].channelName = obj["channelName"] | "";
                count++;
            }
        }
    }

    return results;
}