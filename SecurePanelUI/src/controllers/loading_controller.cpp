#include "loading_controller.h"
#include "ui/ui.h"

void set_loading_text(const char* loadingText) {
    lv_label_set_text(ui_LblLoading, loadingText);
}

void init_loading_screen() {
}

void open_loading_screen(const char* loadingText) {
    set_loading_text(loadingText);
    _ui_screen_change(&ui_Loading, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_Loading_screen_init);
}