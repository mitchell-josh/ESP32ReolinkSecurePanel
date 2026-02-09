#include "auth_handler.h"
#include "api/secure_panel_api.h"

#include <Arduino.h>
#include <ArduinoJson.h>

AuthSession currentSession;

BooleanResult get_result(const char* jsonString);

void authorise(AuthCredentials credentials) {
    RequestModel req;

    // 1. The Base Endpoint (without the ? parameters)
    // We use the macro from your platformio.ini
    req.endpoint = String(SECURE_PANEL_API_URI) + "auth/CheckAlarmCode";

    // 2. Set Query Parameters (?alarmCode=0000)
    req.query["alarmCode"] = credentials.alarmCode;

    // 3. Set Custom Headers
    req.headers["X-Alarm-User"] = credentials.alarmUser;
    req.headers["X-Alarm-Code"] = credentials.alarmCode;

    // 4. Send the Request
    String response = post_data(req);
    
    // 5. Deserialise response
    BooleanResult result = get_result(response.c_str());

    if (result.succeeded) {
        set_authorised(credentials);
    }
}

void set_authorised(AuthCredentials credentials) {
    currentSession.isAuthenticated = true;
    currentSession.authenticatedAt = millis();
    currentSession.credentials = credentials;
}

bool is_authorised() {
    if (!currentSession.isAuthenticated) return false;

    AuthCredentials newCredentials;

    // Check if 5 minutes have passed
    if (millis() - currentSession.authenticatedAt > currentSession.leaseDuration) {
        currentSession.isAuthenticated = false; // Auto-expire
        currentSession.credentials = newCredentials;
        return false;
    }
    return true;
}

AuthCredentials& get_credentials() {
    return currentSession.credentials;
}

BooleanResult get_result(const char* jsonString) {
    JsonDocument doc; 
    DeserializationError error = deserializeJson(doc, jsonString);

    BooleanResult result;

    // If JSON is malformed, log the error and exit to prevent null pointer access
    if (error) {
        Serial.print("JSON Parse failed: ");
        Serial.println(error.f_str());
        return result;
    }
    
    result.succeeded = doc["succeeded"];
    result.value = doc["value"];
    result.errorMessage = doc["errorMessage"];

    return result;
}