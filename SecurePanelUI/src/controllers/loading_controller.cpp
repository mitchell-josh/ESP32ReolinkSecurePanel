#include "loading_controller.h"
#include "ui/ui.h"

void set_loading_text(const char* loadingText) {
    lv_label_set_text(ui_LblLoading, loadingText);
}

void init_loading_screen() {
}

void open_loading_screen(const char* loadingText) {
    set_loading_text(loadingText);
    lv_scr_load_anim(ui_Loading, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, false);
}