// ------------ ESP32 S2 Mini ------------ //

#include <AccelStepper.h>
#include <EEPROM.h>
#include <WiFi.h>
#include <WiFiUdp.h>
#include <NTPClient.h>
#include <WebServer.h>
#include <ArduinoJson.h>

// ------------ Configuration ------------ //
const int stepsPerRevolution = 2048;      // change this to fit the number of steps per revolution

const char* ssid = "TP-Link_1285A";
const char* password = "newcream317";

// Replace with your desired static IP info
IPAddress local_IP(192, 168, 2, 17);      // ESP32's static IP
IPAddress gateway(192, 168, 2, 2);        // Usually your router's IP
IPAddress subnet(255, 255, 255, 0);       // Subnet mask
IPAddress primaryDNS(8, 8, 8, 8);         // Optional: DNS server
IPAddress secondaryDNS(8, 8, 4, 4);       // Optional: backup DNS

const long utcOffsetInSeconds = -5 * 60 * 60;       // adjust for your timezone (e.g. CDT, -5 hours = -18000)
const unsigned long wifiDuration = 10 * 60 * 1000;  // 10 minutes

const float StepsPerFeedingHole = 3413.333332;

#define INIT_PIN_IN 0      // GPIO0 for Start Button
#define LIGHT_PIN_OUT1 16
#define LIGHT_PIN_OUT2 18
#define EEPROM_SIZE 3
#define EEPROM_ADDR 0      // we'll use address 0, 1, 2

// ULN2003 Motor Driver Pins
#define STEPPER_DRV1 12
#define STEPPER_DRV2 11
#define STEPPER_DRV3 9
#define STEPPER_DRV4 7

#define LED_NORMAL   0b00000000000011110000000000001111
#define LED_WIFI1    0b01010101010101010101010101010101
#define LED_WIFI2    0b00001111000011110000111100001111



// ------------ Globals ------------ //
WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, "pool.ntp.org", utcOffsetInSeconds);
WebServer server(80);
AccelStepper stepper(AccelStepper::HALF4WIRE, STEPPER_DRV1, STEPPER_DRV3, STEPPER_DRV2, STEPPER_DRV4);

// String LastSyncDateTime = "";
time_t LastTimeSyncEpoicTime = 0;
unsigned long LastTimeSyncMillis = 0;
unsigned long lastTimeUpdateInMillis = 0;

bool wifiConnected = false;
unsigned long wifiStartMillis = 0;

uint32_t lightSchedule = 0;  // 24-bit schedule stored in 3 bytes
int lastCheckedHour = -1;

int stepper_state = 0;
float stepper_position = 0;

unsigned int led_patten = 0;
short led_state = 0;
unsigned long led_start_ms = 0;

void setup() {
  // initialize the serial port
  // Serial.begin(115200);
  // while (!Serial) {
    // ; // wait for serial port to connect (needed for native USB)
  // }
  // Serial.println("Serial monitor is working!");
  
  // set the speed and acceleration
  stepper.setMaxSpeed(500);
  stepper.setAcceleration(100);
  stepper_position = stepper.currentPosition();
  stepper.disableOutputs(); // Start with motor off
  stepper_state = 0;        // motor off

  pinMode(LED_BUILTIN, OUTPUT);
  pinMode(INIT_PIN_IN, INPUT_PULLUP);  // button pulls LOW when pressed  
  pinMode(LIGHT_PIN_OUT1, OUTPUT);
  
  EEPROM.begin(EEPROM_SIZE);
  saveLightSchedule(0b00000000000011111111111100000000);  // 23,22,21,20..........6,5,4,3,2,1,0
  loadScheduleFromEEPROM();

  led_patten = LED_WIFI1;
  led_start_ms = millis();

  wifiConnected = false;
  connectToWiFi();
  
  // Serial.println("ESP32 Ready");
}

void loop() {

  unsigned long my_now = millis();

  // Handle LED
  if ( my_now - led_start_ms > 250) {
     led_start_ms += 250;
     led_state++;
     if ( led_state >= 32 ) {
       led_state = 0; 
     }

     bool led_TurnOn = (led_patten >> led_state) & 0x01;
     if ( led_TurnOn ) {
       digitalWrite(LED_BUILTIN, HIGH);      
     }
     else {
       digitalWrite(LED_BUILTIN, LOW);      
     }
  }

  // Handle stepper motor
  if (stepper_state == 1) {
    // check current stepper motor position to invert direction
    if (stepper.distanceToGo() == 0 || !stepper.isRunning() ){
      stepper.disableOutputs();  // Cut power to stepper
      stepper_state = 0;
      // Serial.print("Stepper stopped");    
    }  
    else {
      stepper.run();
    }
  }

  // Update datetime every 1s
  if (wifiConnected) {
    // Auto disconnect WiFi after 10 minutes
    if (my_now - wifiStartMillis > wifiDuration) {
      disconnectWiFi();
      led_patten = LED_NORMAL;
    }

    // Handle HTTP server
    server.handleClient();
  }

  // Handle button press
  if (!wifiConnected && digitalRead(INIT_PIN_IN) == LOW) {
    led_patten = LED_WIFI1;
    wifiConnected = false;
    connectToWiFi();
  }

  if (my_now - lastTimeUpdateInMillis >= 60000) {
    lastTimeUpdateInMillis += 60000;

    // Update light based on current hour
    int currentHour = getCurrentHour(my_now);
    updateLightForHour(currentHour);

    // Hour switch
    if (currentHour != lastCheckedHour) {
      lastCheckedHour = currentHour;

      //  Need to feed the fish
      if ( currentHour == 10) {
        moveStepper(StepsPerFeedingHole);
      }
    }
  }
}

void moveStepper(float steps) {
  stepper_position += steps;
  long newTarget = stepper_position;
  
  stepper.enableOutputs();
  stepper.moveTo(newTarget);

  // Serial.print("Stepper move forward to ");    
  // Serial.println(newTarget);      
  stepper_state = 1;
}

int getCurrentHour(unsigned long my_now) {
  time_t rawTimeNow = LastTimeSyncEpoicTime + ( my_now - LastTimeSyncMillis ) / 1000;
  struct tm* timeInfo = localtime(&rawTimeNow);
  return timeInfo->tm_hour;
}

// ------------ WiFi Connect ------------ //
void connectToWiFi() {
  // Serial.println("Connecting to WiFi...");

  // Configure fixed IP before connecting
  if (!WiFi.config(local_IP, gateway, subnet, primaryDNS, secondaryDNS)) {
    // Serial.println("Failed to configure static IP");
  }
    
  WiFi.begin(ssid, password);

  int attempts = 0;
  while (WiFi.status() != WL_CONNECTED && attempts < 20) {
    delay(500);
    // Serial.print(".");
    attempts++;
  }

  if (WiFi.status() == WL_CONNECTED) {

    led_patten = LED_WIFI2;

    // Serial.println("\nWiFi connected.");
    wifiConnected = true;
    wifiStartMillis = millis();

    // Start NTP
    timeClient.begin();
    timeClient.forceUpdate();
    LastTimeSyncEpoicTime = timeClient.getEpochTime();
    LastTimeSyncMillis = millis();

    // Start HTTP server
    setupServer();
    // Serial.println("HTTP server started at IP: " + WiFi.localIP().toString());
  } else {
    // Serial.println("\nFailed to connect.");
  }
}

// ------------ WiFi Disconnect ------------ //
void disconnectWiFi() {
  WiFi.disconnect(true);
  WiFi.mode(WIFI_OFF);
  wifiConnected = false;
  // Serial.println("WiFi disconnected after 10 minutes.");
}

void setupServer() {
  // Handle GET request at "/"
  server.on("/", HTTP_GET, []() {
    // Send HTML page
    server.send(200, "text/html", buildReturnHtml());
  });

  server.on("/lit1on", HTTP_GET, []() {
    digitalWrite(LIGHT_PIN_OUT1, LOW);
    // Serial.println("Turn light on.");
    server.send(200, "text/html", buildReturnHtml());
  });

  server.on("/lit1off", HTTP_GET, []() {
    digitalWrite(LIGHT_PIN_OUT1, HIGH);
    // Serial.println("Turn light off.");
    server.send(200, "text/html", buildReturnHtml());
  });

  server.on("/mv10", HTTP_GET, []() {
    moveStepper(10.0);
    // Serial.println("Turn motor 10 steps.");
    server.send(200, "text/html", buildReturnHtml());
  });

  server.on("/mvhole", HTTP_GET, []() {
    moveStepper(StepsPerFeedingHole);
    // Serial.print("Turn motor ");
    // Serial.print(StepsPerFeedingHole);
    // Serial.println(" steps.");
    server.send(200, "text/html", buildReturnHtml());
  });

  // Handle POST command at "/command"
  server.on("/command", HTTP_POST, handleCommandRequest);

  // Start server
  server.begin();
}

String buildReturnHtml() {
  time_t rawTimeNow = LastTimeSyncEpoicTime + ( millis() - LastTimeSyncMillis ) / 1000;
  String html = "<html><body><p>";
  html += "Server time: " + formatEpochTime(rawTimeNow);
  html += "</p><p><a href='lit1on'>Light On</a><br><a href='lit1off'>Light Off</a><br><br><a href='mv10'>Stepper move 10 steps</a><br><a href='mvhole'>Stepper move 1 hole</a></p></body></html>";
  return html;
}


// ------------ Command Handler ------------ //
void handleCommandRequest() {
  if (server.hasArg("plain")) {
    String body = server.arg("plain");

    // Parse JSON
    StaticJsonDocument<256> doc;
    DeserializationError error = deserializeJson(doc, body);

    if (error) {
      server.send(400, "application/json", "{\"error\":\"Invalid JSON\"}");
      return;
    }

    String cmd = doc["cmd"];
    String param1 = doc["param1"];
    String param2 = doc["param2"];

    // Serial.println("Command Received:");
    // Serial.println("cmd = " + cmd);
    // Serial.println("param1 = " + param1);
    // Serial.println("param2 = " + param2);

    // Handle command
    if (cmd == "RUN") {
      runCommand(param1, param2);
      server.send(200, "application/json", "{\"status\":\"RUN executed\"}");
    } else {
      server.send(200, "application/json", "{\"status\":\"Unknown command\"}");
    }
  } else {
    server.send(400, "application/json", "{\"error\":\"No POST body\"}");
  }
}

// ------------ Example Command Execution ------------ //
void runCommand(const String& param1, const String& param2) {
  // Serial.println("Running command with:");
  // Serial.println("param1: " + param1);
  // Serial.println("param2: " + param2);
  // You can add real logic here to act on the parameters.
}

void updateLightForHour(int hour) {
  if (hour < 0 || hour > 23) return;

  bool shouldTurnOn = (lightSchedule >> hour) & 0x01;
  digitalWrite(LIGHT_PIN_OUT1, shouldTurnOn ? LOW : HIGH);
  // Serial.printf("Hour %d: Light turned %s\n", hour, shouldTurnOn ? "ON" : "OFF");
}

void loadScheduleFromEEPROM() {
  lightSchedule = 0;
  for (int i = 0; i < 3; i++) {
    lightSchedule |= (EEPROM.read(EEPROM_ADDR + i) << (8 * i));
  }
  // Serial.printf("Loaded schedule: 0x%06X\n", lightSchedule);
}

void saveLightSchedule(uint32_t schedule) {
  for (int i = 0; i < 3; i++) {
    EEPROM.write(EEPROM_ADDR + i, (schedule >> (8 * i)) & 0xFF);
  }
  EEPROM.commit();
  
  // Serial.print("schedule ");
  // Serial.print(schedule);
  // Serial.println("saved to EEPROM.");
}

// Function to convert time_t to "MM/dd/yyyy HH:mm:ss" format
String formatEpochTime(time_t epoch_time) {
  struct tm* timeinfo = localtime(&epoch_time);
  char buffer[25];
  
  // Format: MM/dd/yyyy HH:mm:ss
  strftime(buffer, sizeof(buffer), "%m/%d/%Y %H:%M:%S", timeinfo);
  
  return String(buffer);
}
