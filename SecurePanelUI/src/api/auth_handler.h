#ifndef AUTH_HANDLER_H
#define AUTH_HANDLER_H

struct AuthCredentials {
    const char* alarmUser = "Admin";
    const char* alarmCode;
};

struct AuthSession {
    bool isAuthenticated = false;
    unsigned long authenticatedAt = 0;
    AuthCredentials credentials;
    const unsigned long leaseDuration = 5 * 60 * 1000; // 5 minutes
};

void authorise(AuthCredentials credentials);

void set_authorised(AuthCredentials credentials);

bool is_authorised();

AuthCredentials& get_credentials();

#endif // AUTH_HANDLER_H