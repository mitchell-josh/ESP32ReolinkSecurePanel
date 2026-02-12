#include "auth_handler.h"
#include "api/secure_panel_api.h"
#include "api/api_actions.h"
#include "network_task.h"

#include <Arduino.h>
#include <ArduinoJson.h>

// Reference to the global AuthController defined in the main API client file
extern AuthController authController;

// Holds the volatile state of the current user session (in RAM only)
AuthSession currentSession;

// Forward declaration of the internal JSON result parser
BooleanResult get_result(const char* jsonString);

/**
 * Attempts to validate a PIN with the backend.
 * Updates the global LoadingState to drive UI feedback (Spinners/Error icons).
 */
BooleanResult authorise(AuthCredentials credentials) {
    BooleanResult result = authController.checkAlarmCode(credentials);

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

/**
 * Marks the local session as active and timestamps the entry.
 * Stores credentials for subsequent API calls that require headers.
 */
void set_authorised(AuthCredentials credentials) {
    currentSession.isAuthenticated = true;
    currentSession.authenticatedAt = millis();
    currentSession.credentials = credentials;
}

/**
 * Immediate session termination (Logout).
 * Clears credentials from memory for security.
 */
void clear_authorised() {
    currentSession.isAuthenticated = false;
    currentSession.authenticatedAt = 0;
    currentSession.credentials.alarmCode = "";
}

/**
 * Safety check used by the UI and network tasks.
 * Implements an automatic "Lease" expiration (defaulting to 5 minutes).
 */
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

/**
 * Provides a reference to the active credentials for header building.
 */
AuthCredentials& get_credentials() {
    return currentSession.credentials;
}

/**
 * UNIVERSAL PARSER
 * Converts standard backend 'AlarmResult' JSON strings into C++ structs.
 */
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