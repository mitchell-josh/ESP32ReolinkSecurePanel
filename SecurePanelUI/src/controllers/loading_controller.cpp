#include "loading_controller.h"
#include "ui/ui.h"

/**
 * UI SYNC: UPDATE TEXT
 * Updates the label on the loading screen to inform the user
 * exactly what the system is currently doing (e.g., "Authenticating...", "Saving...").
 */
void set_loading_text(const char* loadingText) {
    lv_label_set_text(ui_LblLoading, loadingText);
}

/**
 * INITIALIZATION
 * Placeholder for any specific loading screen setup (animations, etc.).
 */
void init_loading_screen() {
}

/**
 * THE LOADING DISPATCHER
 * Transitions the display to the Loading screen with a fade animation.
 * Typically called by the 'run_with_loading' function in the network task system.
 * * @param loadingText The descriptive text to show during the operation.
 */
void open_loading_screen(const char* loadingText) {
    set_loading_text(loadingText);
    lv_scr_load_anim(ui_Loading, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, false);
}