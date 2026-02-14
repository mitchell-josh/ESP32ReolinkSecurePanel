#define TOUCH_MODULES_CST_MUTUAL

#include <Arduino_GFX_Library.h>
#include <ArduinoJson.h>
#include <lvgl.h>
#include <WiFi.h>
#include <esp_wifi.h>
#include "TouchLib.h"
#include "controllers/camera_select_controller.h"
#include "controllers/loading_controller.h"
#include "controllers/pin_controller.h"
#include "controllers/settings_controller.h"
#include "controllers/camera_settings_controller.h"
#include "controllers/error_controller.h"
#include "controllers/ui_workflows.h"
#include "api/secure_panel_api.h"
#include "api/network_task.h"

// External C linkage for SquareLine Studio generated UI files
// This allows C++ to access variables and functions defined in the UI C files
extern "C" {
    #include "ui/ui.h"
}

extern UIWorkflow pinWorkflow;

#define TOUCH_SDA 1
#define TOUCH_SCL 3
#define TOUCH_INT 4
#define TOUCH_RST 2
#define TOUCH_READ_FROM_INTERRNUPT 1 

/* Hardware & Buffer Setup 
 * Configures the SPI bus and the specific ST7789 display controller
 */
Arduino_DataBus *bus = new Arduino_ESP32SPI(41, 42, 40, 45, -1);
Arduino_GFX *gfx = new Arduino_ST7789(bus, 39, 1, true);

static const uint32_t screenWidth  = 320;
static const uint32_t screenHeight = 240;

// LVGL Drawing Buffers: Used to render pixels before sending to the display
static lv_disp_draw_buf_t draw_buf;
static lv_color_t buf[screenWidth * 10]; // buffer 10 lines of screen
TouchLib touch(Wire, TOUCH_SDA, TOUCH_SCL, CTS328_SLAVE_ADDRESS);

// Track ui initialisation state
bool ui_initialised = false;

const char* ssid = "YOUR_USERNAME";
const char* password = "YOUR_PASSWORD";

void setup_wifi() {    
    // Optional: Set hostname so it shows up in your router as a specific name
    WiFi.setHostname("Secure-Panel-S3");
    
    WiFi.begin(ssid, password);

    int attempt = 0;
    // Wait for connection with a 10-second timeout
    while (WiFi.status() != WL_CONNECTED && attempt < 20) {
        delay(500);
        attempt++;
    }

    if (WiFi.status() == WL_CONNECTED) {
      // Disable WiFi Power Saving to keep the radio "Hot"
      // This is critical for instant responsiveness and avoiding -1 errors
      esp_wifi_set_ps(WIFI_PS_NONE);     
    } else {
      open_error_screen("WiFi Connection Failed. Please check your settings and restart the device.", [](){});
    }
}

void my_disp_flush(lv_disp_drv_t *disp, const lv_area_t *area, lv_color_t *color_p);

/**
 * @brief LVGL Flush Callback: Transfers the rendered internal buffer to the LCD.
 * This is the bridge between the LVGL graphics library and the Arduino_GFX driver.
 */
void my_disp_flush(lv_disp_drv_t *disp, const lv_area_t *area, lv_color_t *color_p) {
    uint32_t w = (area->x2 - area->x1 + 1);
    uint32_t h = (area->y2 - area->y1 + 1);

    // Push pixels to display
    gfx->draw16bitBeRGBBitmap(area->x1, area->y1, (uint16_t *)&color_p->full, w, h);
    
    // Inform LVGL flushing complete
    lv_disp_flush_ready(disp);
}

bool get_int = false;

void scan_i2c_device(TwoWire &i2c)
{
  uint8_t error;
  for (int j = 0; j < 0x80; j += 0x10)
  {
    for (int i = 0; i < 0x10; i++)
    {
      Wire.beginTransmission(i | j);
      error = Wire.endTransmission();
    }
  }
}

void touchpad_read(lv_indev_drv_t *indev_driver, lv_indev_data_t *data) {
    bool touched = false;

    #if (TOUCH_READ_FROM_INTERRNUPT)
    if (get_int) {
        get_int = 0;
        touched = touch.read();
    }
    #else
    touched = touch.read();
    #endif

    if (touched) {
        uint8_t n = touch.getPointNum();
        if (n > 0) {
            TP_Point t = touch.getPoint(0); // Take the first touch point for LVGL
            
            // Set the coordinates
            data->point.x = t.y; 
            data->point.y = screenHeight - t.x;
            data->state = LV_INDEV_STATE_PR; // Pressed
            return; 
        }
    }
    
    // If no touch detected or touch was released
    data->state = LV_INDEV_STATE_REL; // Released
}

void run_ready_check() {
  api_ready_check();
}

void setup() {    
    // Increase serial buffer to accomodate high-frequency JSON updates
    Serial.setRxBufferSize(1024);

    // Set baud rate (should match .NET service)
    Serial.begin(9600);
    
    // Hardware backlight control
    pinMode(5, OUTPUT);
    digitalWrite(5, HIGH); 

    // Initialise display hardware
    gfx->begin();

    // Initialise LVGL core
    lv_init();

    pinMode(TOUCH_RST, OUTPUT);
    digitalWrite(TOUCH_RST, 0);
    delay(200);
    digitalWrite(TOUCH_RST, 1);
    delay(200);
    Wire.begin(TOUCH_SDA, TOUCH_SCL, 400000);
    scan_i2c_device(Wire);

    touch.init();
    #if (TOUCH_READ_FROM_INTERRNUPT)
      attachInterrupt(
        TOUCH_INT,
        []
        {
          get_int = 1;
        },
        CHANGE);
    #endif 

    // Setup LVGL display driver
    lv_disp_draw_buf_init(&draw_buf, buf, NULL, screenWidth * 10);
    static lv_disp_drv_t disp_drv;
    lv_disp_drv_init(&disp_drv);
    disp_drv.hor_res = screenWidth;
    disp_drv.ver_res = screenHeight;
    disp_drv.flush_cb = my_disp_flush;
    disp_drv.draw_buf = &draw_buf;
    lv_disp_drv_register(&disp_drv);

    static lv_indev_drv_t indev_drv;
    lv_indev_drv_init(&indev_drv);
    indev_drv.type = LV_INDEV_TYPE_POINTER;
    indev_drv.read_cb = touchpad_read; 
    lv_indev_drv_register(&indev_drv);

    // Build UI
    ui_init();
    ui_initialised = true;

    if (ui_initialised) {
      init_pin_controller();
      init_loading_screen();
      init_settings_controller();
      init_camera_select_controller();
      init_camera_settings_controller();
      init_error_controller();
    }

    setup_wifi();

    pinWorkflow.onSuccess = []() {
      // Give the system 200ms to finish WiFi/API checks before jumping to the PIN screen
      lv_timer_t * t = lv_timer_create([](lv_timer_t * timer) {
        open_pin_screen(PinMode::MODE_UNLOCK);
        lv_timer_del(timer);
      }, 200, NULL); 
     };

    run_with_loading(run_ready_check, "Booting...");
}

void loop() {
    // Maintain LVGL internal clock, tell LVGL 5ms has passed.
    lv_tick_inc(5);

    // Run controller monitors
    if (ui_initialised) {
      monitor_pin_network_task();
      monitor_camera_select_network_task();
      monitor_camera_settings_network_task();
    }
    // Process UI tasks (update UI, animations and transitions)
    lv_timer_handler();

    // Yield small delay to keep system stable
    delay(5);
}
