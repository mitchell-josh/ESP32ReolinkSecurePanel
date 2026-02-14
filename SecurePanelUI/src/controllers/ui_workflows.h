#ifndef UI_WORKFLOWS_H
#define UI_WORKFLOWS_H

#include <functional>

/**
 * @brief Generic callback for UI transitions and network completion events.
 */
using UIWorkflowCallback = std::function<void()>;

/**
 * @brief Encapsulates navigation paths for UI events.
 * This structure allows any controller to define what happens after a 
 * background task (like an API call) finishes.
 */
struct UIWorkflow {
    UIWorkflowCallback onSuccess;
    UIWorkflowCallback onFailure;

    // Helper to reset both callbacks to null   
    void clear() {
        onSuccess = nullptr;
        onFailure = nullptr;
    }
};

// Global workflow instances used by different parts of the app
extern UIWorkflow pinWorkflow;
extern UIWorkflow cameraSelectWorkflow;
extern UIWorkflow cameraSettingsWorkflow;

#endif // UI_WORKFLOWS_H