#include "loading_controller.h"
#include "ui/ui.h"

void init_loading_screen() {
}

void open_loading_screen() {
    _ui_screen_change(&ui_Loading, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_Loading_screen_init);
}