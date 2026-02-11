#include "auth_handler.h"
#include "api/secure_panel_api.h"
#include "api/api_actions.h"
#include "network_task.h"

#include <Arduino.h>
#include <ArduinoJson.h>

extern Auth auth;

AuthSession currentSession;

BooleanResult get_result(const char* jsonString);

BooleanResult authorise(AuthCredentials credentials) {
    BooleanResult result = auth.checkAlarmCode(credentials);

    if (result.succeeded && result.value == true) {
        set_authorised(credentials);
        loadingState = LoadingState::SUCCESS; 
    }
    else {
        loadingState = LoadingState::ERROR; 
    }

    vTaskDelay(10);
    return result;
}

void set_authorised(AuthCredentials credentials) {
    currentSession.isAuthenticated = true;
    currentSession.authenticatedAt = millis();
    currentSession.credentials = credentials;
}

void clear_authorised() {
    currentSession.isAuthenticated = false;
    currentSession.authenticatedAt = 0;
    currentSession.credentials.alarmCode = "";
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
    // Initialize with safe defaults!
    BooleanResult result;
    result.succeeded = false;
    result.value = false;
    result.errorMessage = "Unknown Error";

    if (jsonString == nullptr || strlen(jsonString) == 0) {
        return result; 
    }

    JsonDocument doc; 
    DeserializationError error = deserializeJson(doc, jsonString);

    if (error) {
        Serial.print("JSON Parse failed: ");
        Serial.println(error.f_str());
        return result; // Now returns the {false, false} default safely
    }
    
    // Use the '|' operator to provide fallbacks for missing JSON keys
    result.succeeded = doc["succeeded"] | false;
    result.value = doc["value"] | false;
    
    const char* msg = doc["errorMessage"];
    if(msg) result.errorMessage = msg;

    return result;
}