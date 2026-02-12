#ifndef NETWORK_TASK_H
#define NETWORK_TASK_H

#include <cstdint>
#include <functional>

/**
 * Standard alias for the function to be executed in the background.
 * Supports lambdas, allowing for inline logic passing from the UI layer.
 */
typedef std::function<void()> WorkerFunc;

/**
 * Parameter packet passed across the core boundary.
 * Contains everything the background task needs to execute and report status.
 */
struct NetworkTaskParams {
    WorkerFunc action;       // The logic to execute (e.g., an API call)
    const char* loadingText; // The string to be displayed on the UI overlay
    uint32_t timeoutMs;      // Maximum time allowed before force-timing out
};

/**
 * State machine identifiers for the visual feedback system.
 * Used by the UI thread to decide which icons/animations to render.
 */
enum LoadingState {
    STATE_TIMEOUT, // Operation took too long (Network/Server lag)
    IDLE,          // No active background process
    LOADING,       // Request is currently in flight
    SUCCESS,       // Operation completed and returned positive result
    ERROR          // Operation failed or returned negative result
};

/**
 * Global synchronization flag.
 * 'volatile' prevents compiler optimization, ensuring changes made on Core 0 
 * are immediately visible to the UI loop on Core 1.
 */
extern volatile LoadingState loadingState;

/**
 * The entry point for the FreeRTOS task.
 * @param pvParameters Pointer to a NetworkTaskParams struct on the heap.
 */
void network_task(void * pvParameters);

/**
 * Public dispatcher that handles task creation, memory allocation, 
 * and core pinning (usually pins network tasks to Core 0).
 */
void run_with_loading(WorkerFunc func, const char* message);

#endif // NETWORK_TASK_H