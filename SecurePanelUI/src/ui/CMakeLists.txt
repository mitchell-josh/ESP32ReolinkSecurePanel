SET(SOURCES screens/ui_PinEntry.c
    screens/ui_SettingsLanding.c
    screens/ui_CameraSelect.c
    screens/ui_CameraSettings.c
    screens/ui_Error.c
    screens/ui_Loading.c
    ui.c
    components/ui_comp_hook.c
    ui_helpers.c)

add_library(ui ${SOURCES})
