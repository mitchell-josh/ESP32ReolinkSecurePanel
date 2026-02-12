#include "api/secure_panel_api.h"

/**
 * Executes an HTTP GET request.
 * Automatically serializes JsonObject query parameters into a URL-encoded string.
 */
String get_data(RequestModel& req) {
    HTTPClient http;
    http.setConnectTimeout(10000); // 3 seconds to find the server
    
    // Append query params to URL
    String fullUrl = String(req.endpoint.c_str());
    if (!req.query.isNull()) {
        fullUrl += "?";
        JsonObject params = req.query.as<JsonObject>();
        int count = 0;
        for (JsonPair p : params) {
            fullUrl += String(p.key().c_str()) + "=" + p.value().as<const char*>() + "&";
            count++;
            if (count != params.size()) fullUrl += "&";
        }
    }

    delay(100); 
    yield();

    http.begin(fullUrl);
    http.setTimeout(20000); // Give it 5 seconds to respond
    http.setReuse(false); // Disable connection reuse
    
    // Add Headers
    JsonObject headers = req.headers.as<JsonObject>();
    for (JsonPair p : headers) {
        http.addHeader(p.key().c_str(), p.value().as<const char*>());
    }
    http.addHeader("Connection", "close"); // Tell .NET to drop the socket immediately

    int httpResponseCode = http.GET();
    String response = "{}";

    // Only read body if the server returned a valid code (> 0)
    if (httpResponseCode > 0) {
        response = http.getString();
    }

    http.end();
    delay(10);
    return response;
}

/**
 * Executes an HTTP POST request.
 * Handles both JSON body serialization and query parameters simultaneously.
 */
String post_data(RequestModel& req) {
    HTTPClient http;
    http.setConnectTimeout(10000); // 3 seconds to find the server
    
    // Append query params to URL
    String fullUrl = String(req.endpoint.c_str());
    if (!req.query.isNull()) {
        fullUrl += "?";
        JsonObject params = req.query.as<JsonObject>();
        int count = 0;
        for (JsonPair p : params) {
            String value = p.value().as<String>();
            fullUrl += String(p.key().c_str()) + "=" + value;
            count++;
            if (count != params.size()) fullUrl += "&";
        }
    }

    Serial.println(fullUrl);
    delay(100); 
    yield();

    // Initialize URL
    http.begin(fullUrl);
    http.setTimeout(20000); // Give it 5 seconds to respond
    http.setReuse(false); // Disable connection reuse
    http.addHeader("Connection", "close"); // Tell .NET to drop the socket immediately
    
    // Add Headers from JsonDocument
    JsonObject headers = req.headers.as<JsonObject>();
    if (headers.size() > 0) {
        for (JsonPair p : headers) {
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
    }

    http.end(); // CRITICAL: Move this as high as possible
    delay(10);
    return response;
}

/**
 * Blocking boot-sequence check.
 * Continually pings the 'auth/test' endpoint until the backend is reachable.
 * Useful for ensuring the device waits for the server to spin up after a power cut.
 */
void api_ready_check() {
    bool isOnline = false;
    int attempts = 0;

    while (!isOnline) {
        attempts++;

        RequestModel req;
        req.endpoint = String(SECURE_PANEL_API_URI) + "auth/test";

        String response = get_data(req);

        if (response != "{}" && response.length() > 0) {
            isOnline = true;
        } else {
            delay(2000); 
        }

        if (isOnline) {
            break;
        }

        // Safety break after 120 seconds (30 attempts * 2s)
        if (attempts > 60) {
            break;
        }
    }
}
