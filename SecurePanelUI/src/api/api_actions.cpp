#include "api_actions.h"
#include "api/auth_handler.h"
#include "api/secure_panel_api.h"

#include <Arduino.h>

Auth auth{};

Auth::Auth() {}

BooleanResult get_result(const char* jsonString);

BooleanResult Auth::checkAlarmCode(AuthCredentials credentials) {
    RequestModel request;
    request.endpoint = String(SECURE_PANEL_API_URI) + "auth/CheckAlarmCode";
    request.query["alarmCode"] = credentials.alarmCode;
    request.headers["X-Alarm-User"] = credentials.alarmUser;
    request.headers["X-Alarm-Code"] = credentials.alarmCode;
    String response = post_data(request);
    return get_result(response.c_str());
}

BooleanResult Auth::changeAlarmCode(String newAlarmCode) {
    AuthCredentials credentials = get_credentials();
    RequestModel request;
    request.endpoint = String(SECURE_PANEL_API_URI) + "auth/ChangeAlarmCode";
    request.query["newAlarmCode"] = newAlarmCode;
    request.headers["X-Alarm-User"] = credentials.alarmUser;
    request.headers["X-Alarm-Code"] = credentials.alarmCode;
    String response = post_data(request);
    return get_result(response.c_str());
}