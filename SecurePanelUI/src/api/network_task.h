#ifndef NETWORK_TASK_H
#define NETWORK_TASK_H

#include <cstdint>

typedef void(*WorkerFunc)();

struct NetworkTaskParams {
    WorkerFunc action;
    const char* loadingText;
    uint32_t timeoutMs;
};

enum LoadingState {
    STATE_TIMEOUT,
    IDLE,
    LOADING,
    SUCCESS
};

void network_task(void * pvParameters);

void run_with_loading(WorkerFunc func, const char* message);

#endif // NETWORK_TASK_H