#include "camera_settings_controller.h"
#include "ui/ui.h"

static Channel currentChannel;
static AlarmScheme currentAlarmScheme;

void init_camera_settings_controller() {
}

void open_camera_settings_screen(Channel channel, AlarmScheme alarmScheme) {
    currentChannel = channel;
    currentAlarmScheme = alarmScheme;
    _ui_screen_change(&ui_CameraSettings, LV_SCR_LOAD_ANIM_FADE_ON, 200, 0, &ui_CameraSettings_screen_init);
}