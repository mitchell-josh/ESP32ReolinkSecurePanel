#ifndef LOADING_CONTROLLER_H
#define LOADING_CONTROLLER_H

/**
 * Prepares the loading screen assets.
 * Usually called during the system boot sequence to ensure the 
 * UI objects are instantiated and ready in memory.
 */
void init_loading_screen();

/**
 * Triggers the transition to the Loading overlay.
 * This is a non-blocking UI call, but it visually blocks user input 
 * by placing a full-screen layer over the interactive dashboard.
 * * @param loadingText The string literal to display (e.g., "Connecting to NVR...")
 */
void open_loading_screen(const char* loadingText);

#endif // LOADING_CONTROLLER_H