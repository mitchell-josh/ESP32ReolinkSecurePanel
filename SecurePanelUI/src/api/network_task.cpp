#include "network_task.h"
#include "controllers/loading_controller.h"

#include <Arduino.h>

volatile LoadingState loadingState = LoadingState::IDLE;

void network_task(void * pvParameters) {
    NetworkTaskParams* params = (NetworkTaskParams*)pvParameters;
    loadingState = LoadingState::LOADING;

    uint32_t startTime = xTaskGetTickCount();
    uint32_t timeoutTicks = pdMS_TO_TICKS(params->timeoutMs);

    if (params->action != nullptr) {
        params->action();
    }

    // After the function returns, check if it took too long
    if ((xTaskGetTickCount() - startTime) > timeoutTicks) {
        Serial.println("Setting loading state to STATE_TIMEOUT");
        loadingState = LoadingState::STATE_TIMEOUT;
    } else if (loadingState == LOADING) {
        Serial.println("Setting loading state to SUCCESS");
        loadingState = SUCCESS;
    }

    delete params;
    vTaskDelete(NULL);
}

void run_with_loading(WorkerFunc func, const char* message) {
    NetworkTaskParams* params = new NetworkTaskParams();
    params->action = func;
    params->loadingText = message;
    params->timeoutMs = 300000;

    open_loading_screen(message);

    xTaskCreatePinnedToCore(
        network_task,
        "network_task",
        10000,
        params,
        1,
        NULL,
        0
    );
}