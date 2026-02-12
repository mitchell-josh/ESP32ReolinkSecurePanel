#include "network_task.h"
#include "controllers/loading_controller.h"

#include <Arduino.h>

/**
 * Global volatile state used by the UI thread to determine 
 * which icon or message to render (Spinner, Checkmark, or Warning).
 */
volatile LoadingState loadingState = LoadingState::IDLE;

/**
 * THE BACKGROUND WORKER (FreeRTOS Task)
 * This function runs on Core 0. It executes the network request
 * and manages the lifecycle of the loading state.
 */
void network_task(void * pvParameters) {
    // Recover the parameters passed from the main thread
    NetworkTaskParams* params = (NetworkTaskParams*)pvParameters;
    loadingState = LoadingState::LOADING;

    // Start the stopwatch
    uint32_t startTime = xTaskGetTickCount();
    uint32_t timeoutTicks = pdMS_TO_TICKS(params->timeoutMs);

    // Run API call
    if (params->action != nullptr) {
        params->action();
    }

    // After the function returns, check if it took too long
    if ((xTaskGetTickCount() - startTime) > timeoutTicks) {
        loadingState = LoadingState::STATE_TIMEOUT;
    } else if (loadingState == LOADING) {
        loadingState = SUCCESS;
    }

    // Cleanup
    delete params;
    vTaskDelete(NULL);
}

/**
 * THE TASK DISPATCHER
 * Called by the UI thread to trigger an async operation.
 * @param func The specific function to run (e.g., [](){ authController.test(); })
 * @param message The text to display on the loading overlay
 */
void run_with_loading(WorkerFunc func, const char* message) {
    // Prepare parameters for the background core
    NetworkTaskParams* params = new NetworkTaskParams();
    params->action = func;
    params->loadingText = message;
    params->timeoutMs = 300000;

    // Immediately switch the screen to the loading overlay
    open_loading_screen(message);

    // Spawn the background process on Core 0
    // Stack size: 10000 bytes (sufficient for JSON parsing and SSL)
    // Priority: 1
    xTaskCreatePinnedToCore(
        network_task,       // Function to run
        "network_task",     // Name for debugging
        10000,              // Stack depth
        params,             // Arguments passed to task
        1,                  // Priority
        NULL,               // Task handle
        0                   // Core ID (0 is the system/network core)
    );
}