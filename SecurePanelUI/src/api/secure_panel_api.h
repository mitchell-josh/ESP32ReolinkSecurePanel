#ifndef SECURE_PANEL_API_H
#define SECURE_PANEL_API_H

#include <ArduinoJson.h>
#include <HTTPClient.h>
#include <WiFi.h>

struct RequestModel {
    String endpoint;
    JsonDocument body;
    JsonDocument headers;
    JsonDocument query;
    int timeout = 5000;
};

struct BooleanResult {
    bool succeeded = false;
    bool value = false;
    const char* errorMessage = "";
};

String get_data(RequestModel& req);

String post_data(RequestModel& req);

#endif // SECURE_PANEL_API_H