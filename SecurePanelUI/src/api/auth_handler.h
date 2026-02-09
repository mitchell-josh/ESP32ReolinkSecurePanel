#ifndef AUTH_HANDLER_H
#define AUTH_HANDLER_H

#include <Arduino.h>

struct AuthCredentials {
    String alarmUser = "Admin";
    String alarmCode = "";
};

struct AuthSession {
    bool isAuthenticated = false;
    unsigned long authenticatedAt = 0;
    AuthCredentials credentials;
    const unsigned long leaseDuration = 5 * 60 * 1000; // 5 minutes
};

void authorise(AuthCredentials credentials);

void set_authorised(AuthCredentials credentials);

void clear_authorised();

bool is_authorised();

AuthCredentials& get_credentials();

#endif // AUTH_HANDLER_H