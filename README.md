# 🛡️ ESP32 Reolink Secure Panel

A dedicated hardware control panel for Reolink security cameras. This project uses an **ESP32 HMI** display (CrowPanel) to communicate with a custom **.NET 10 Web API**, allowing for secure, abstracted control of camera functions like the Siren and Spotlight without exposing camera credentials directly to the ESP32 hardware. Currently only supports **Argus 3E** cameras.

---

## 🏗️ System Architecture

* **ESP32 User Interface**: Written in C++ using **PlatformIO**. It handles the touch UI and sends lightweight JSON requests to the backend.
* **Backend API**: A high-performance **.NET 10** service that manages Reolink authentication (Token/Session handling) and relays commands to the camera.
* **SQLite Database**: Built-in database for the API to manage state and logs.



---

## 🚀 1. Backend Deployment (Docker)

The backend is containerized for easy deployment on a home server, Raspberry Pi, or NAS.

### Setup
1.  **Login to GHCR**:
    ```bash
    echo YOUR_PAT_TOKEN | docker login ghcr.io -u mitchell-josh --password-stdin
    ```
2.  **Docker Compose**:
    Create a `docker-compose.yml` file:
    ```yaml
    services:
      secure-panel-api:
        image: ghcr.io/mitchell-josh/esp32reolinksecurepanel:latest
        container_name: secure-panel-api
        restart: unless-stopped
        ports:
          - "8080:8080"
        environment:
          - ASPNETCORE_ENVIRONMENT=Production
          - ConnectionStrings__DefaultConnection=Data Source=/app/data/SecurePanel.db
          - Logging__LogLevel__ReolinkAPI=Debug
          - Logging__LogLevel__SecurePanelAPI=Debug
          - Settings__Username=admin
          - Settings__Password=your_camera_password
          - Settings__ReolinkURL=your_reolink_url
        volumes:
          - ./data:/app/data:
          
      watchtower:
        image: containrrr/watchtower
        volumes:
          - /var/run/docker.sock:/var/run/docker.sock
        command: --interval 300 --cleanup
    ```
3.  **Launch**: `docker compose up -d`

---

## 📟 2. Firmware Setup (ESP32)

To keep your network credentials secure, you should build the firmware locally using **PlatformIO**.

### Prerequisites
* [VS Code](https://code.visualstudio.com/) + [PlatformIO IDE Extension](https://platformio.org/platformio-ide).
* ESP32-based HMI Display (e.g., CrowPanel).

### Build Instructions
1.  **Clone the Repository**:
    ```bash
    git clone [https://github.com/mitchell-josh/ESP32ReolinkSecurePanel.git](https://github.com/mitchell-josh/ESP32ReolinkSecurePanel.git)
    cd ESP32ReolinkSecurePanel/SecurePanelUI
    ```
2.  **Configure Credentials**:
    Open `platformio.ini` and locate the `build_flags`. Update these with your local settings:
    ```ini
    build_flags = 
        -D WIFI_SSID=your_wifi_ssid
        -D WIFI_PASSWORD=your_wifi_password
        -D SECURE_PANEL_API_URI=your_api_uri
    ```
3.  **Flash the Device**:
    * Connect your ESP32 via USB.
    * Click the **PlatformIO: Build** (checkmark) in the bottom status bar.
    * Click the **PlatformIO: Upload** (right arrow) to flash the firmware.

---

## 🖨️ 3D Printed Enclosure

To give the Secure Panel a professional finish, you can print a custom enclosure. The design below is specifically tailored for the **Elecrow CrowPanel 7.0" HMI**.

* **Model Link**: [Elecrow CrowPanel 7.0" Case (Printables)]([https://www.printables.com/model/1476807-elecrow-crowpanel-advance-7-enclosure](https://www.printables.com/model/1600239-esp32-s3-28-touch-lcd-alarm-panel))

### Suggested Print Settings:
* **Material**: PLA.
* **Layer Height**: 0.2mm.
* **Infill**: 20% (Gyroid recommended).

### Hardware Required:
* 4x M2.5 screws for the display mount.
* 4x M2.5 screws for the side plate.

## 🛠️ Development & Maintenance

### Git Cleanup
This repository is configured to ignore build artifacts. If you encounter issues with `bin/` or `obj/` folders being tracked, run the following to purge the Git index:
```bash
git rm -r --cached "**/bin/" "**/obj/"
git commit -m "chore: clear build artifacts from tracking"
