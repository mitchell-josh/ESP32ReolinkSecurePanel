SET(SOURCES screens/ui_PinEntry.c
    screens/ui_SettingsLanding.c
    screens/ui_CameraSelect.c
    screens/ui_CameraSettings.c
    ui.c
    components/ui_comp_hook.c
    ui_helpers.c
    fonts/ui_font_FontAwesome.c)

add_library(ui ${SOURCES})
