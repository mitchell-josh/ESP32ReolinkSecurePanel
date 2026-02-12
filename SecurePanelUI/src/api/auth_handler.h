#ifndef AUTH_HANDLER_H
#define AUTH_HANDLER_H

#include "secure_panel_api.h"

#include <Arduino.h>

/**
 * Encapsulates the identity required for API requests.
 * Defaulted to "Admin" to match the primary system user.
 */
struct AuthCredentials {
    String alarmUser = "Admin"; // The username to be sent in 'X-Alarm-User'
    String alarmCode = "";      // The plain-text PIN to be sent in 'X-Alarm-Code'
};

/**
 * Manages the transient state of a logged-in session.
 * Includes automatic expiration logic (TTL) for security.
 */
struct AuthSession {
    bool isAuthenticated = false;       // Track if the current PIN has been verified
    unsigned long authenticatedAt = 0; // Timestamp of last successful login (millis)
    AuthCredentials credentials;       // Active credentials used for header injection
    
    // Constant defining how long the panel stays unlocked without activity
    // 5 minutes * 60 seconds * 1000 milliseconds
    const unsigned long leaseDuration = 5 * 60 * 1000;
};

/**
 * External interface for the session manager.
 * Implemented in the corresponding .cpp file.
 */

// Triggers the network-based PIN verification and updates the global session
BooleanResult authorise(AuthCredentials credentials);

// Manually upgrades the local state to "Authenticated" (internal use)
void set_authorised(AuthCredentials credentials);

// Resets the session state and wipes the stored PIN
void clear_authorised();

// Checks if the session is still valid based on the lease duration
bool is_authorised();

// Retrieves the active credentials for use in API request headers
AuthCredentials& get_credentials();

#endif // AUTH_HANDLER_H