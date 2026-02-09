#include "api/secure_panel_api.h"

String get_data(RequestModel& req) {
    HTTPClient http;
    
    // Append query params to URL
    String fullUrl = String(req.endpoint.c_str());
    if (!req.query.isNull()) {
        fullUrl += "?";
        JsonObject params = req.query.as<JsonObject>();
        for (JsonPair p : params) {
            fullUrl += String(p.key().c_str()) + "=" + p.value().as<const char*>() + "&";
        }
    }

    Serial.println("Sending request....");
    Serial.println(fullUrl);

    http.begin(fullUrl);
    
    // Add Headers
    JsonObject headers = req.headers.as<JsonObject>();
    for (JsonPair p : headers) {
        http.addHeader(p.key().c_str(), p.value().as<const char*>());
    }

    int httpResponseCode = http.GET();
    String response = "{}";

    if (httpResponseCode > 0) {
        response = http.getString();
    }

    http.end();
    return response;
}

String post_data(RequestModel& req) {
    HTTPClient http;
    
    // Append query params to URL
    String fullUrl = String(req.endpoint.c_str());
    if (!req.query.isNull()) {
        fullUrl += "?";
        JsonObject params = req.query.as<JsonObject>();
        int count = 0;
        for (JsonPair p : params) {
            fullUrl += String(p.key().c_str()) + "=" + p.value().as<const char*>();
            count++;
            if (count != params.size()) fullUrl += "&";
        }
    }

    Serial.println("Sending request....");
    Serial.println(fullUrl);

    // Initialize URL
    http.begin(fullUrl);
    
    // Add Headers from JsonDocument
    JsonObject headers = req.headers.as<JsonObject>();
    if (headers.size() > 0) {
        for (JsonPair p : headers) {
            Serial.println(p.key().c_str());
            Serial.println(p.value().as<const char*>());
            http.addHeader(p.key().c_str(), p.value().as<const char*>());
        }
    }

    // Add body and send request
    int httpResponseCode;
    if (req.body != NULL && req.body.size() > 0) {
        http.addHeader("Content-Type", "application/json");
        String jsonBody;
        serializeJson(req.body, jsonBody);
        httpResponseCode = http.POST(jsonBody);
    }
    else {
        http.addHeader("Content-Length", "0");
        httpResponseCode = http.POST("");
    }

    String response = "{}";

    if (httpResponseCode > 0) {
        response = http.getString();
        Serial.printf("HTTP Response code: %d\n", httpResponseCode);
    } else {
        Serial.printf("Error code: %d\n", httpResponseCode);
    }

    http.end();
    return response;
}

