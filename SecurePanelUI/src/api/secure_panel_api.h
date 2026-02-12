#ifndef SECURE_PANEL_API_H
#define SECURE_PANEL_API_H

#include <ArduinoJson.h>
#include <HTTPClient.h>
#include <WiFi.h>

/**
 * A container for all data required to execute an HTTP request.
 * Designed to handle GET and POST operations interchangeably.
 */
struct RequestModel {
    String endpoint;      // The full URL destination
    JsonDocument body;    // Data to be serialized into the JSON body (POST)
    JsonDocument headers; // Custom HTTP headers (e.g., security tokens)
    JsonDocument query;   // Key-value pairs for URL query strings (?id=1)
    int timeout = 5000;   // Default request timeout in milliseconds
};

/**
 * Standardized response for operations that return a simple status.
 * Maps directly to the backend's 'AlarmResult<bool>' type.
 */
struct BooleanResult {
    bool succeeded = false;      // Did the request reach the server and process?
    bool value = false;          // The actual boolean answer from the logic
    const char* errorMessage = ""; // Descriptive text if 'succeeded' is false
};

/**
 * Standardized response for operations returning complex data.
 * The 'value' document can be cast to Objects or Arrays as needed.
 */
struct DataResult {
    bool succeeded = false;
    JsonDocument value;          // The payload (e.g., list of channels or settings)
    const char* errorMessage = "";
};

/**
 * LOW-LEVEL TRANSPORT PROTOTYPES
 * Implemented in api_actions.cpp
 */

// Executes a GET request with query parameter serialization
String get_data(RequestModel& req);

// Executes a POST request with body serialization and header injection
String post_data(RequestModel& req);

// Performs a blocking check to ensure the server is online before continuing
void api_ready_check();

#endif // SECURE_PANEL_API_H