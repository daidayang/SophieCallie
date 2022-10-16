//  https://randomnerdtutorials.com/esp32-web-server-slider-pwm/

#include <EEPROM.h>
#include <WiFi.h>
#include <AsyncTCP.h>
#include <ESPAsyncWebServer.h>
#include <Arduino.h>

// Replace with your network credentials
const char* ssid = "NETGEAR56";
const char* password = "newcream317";

#define RightFrontFWD 27
#define RightFrontBWD 26
#define LeftFrontFWD  14
#define LeftFrontBWD  12

short LeftStartLevel =  100;
short RightStartLevel = 104;

int motorspeed = 64;

String Serial0InputString   = "";        // a String to hold incoming data
bool   Serial0InputComplete = false;     // whether the string is complete

char serialdata[3];

unsigned long LastOneSecTimeInMs;
short LargePulseWidthInMs = 50;
short LargeFulseSpeed     = 100;
short LargeFulseMaxCountPerHalfSec = 8;

short PendingMoves_Left  = 0;
short PendingMoves_Right = 0;

short RunStep_Left  = 0;
short RunStep_Right = 0;

unsigned long DelayStartTimeInMs_Left;
unsigned long DelayStartTimeInMs_Right;

int PulseGapTimeInMs_Left;
int PulseGapTimeInMs_Right;

void ReadSettingFromEEPROM() {
  LeftStartLevel               = EEPROM.readShort(0);
  RightStartLevel              = EEPROM.readShort(4);
  LargePulseWidthInMs          = EEPROM.readShort(8);
  LargeFulseSpeed              = EEPROM.readShort(12);      //  PWM duty cycle
  LargeFulseMaxCountPerHalfSec = EEPROM.readShort(16);      //  

  if ( LargePulseWidthInMs < 0 ) LargePulseWidthInMs = 0;
  if ( LargeFulseSpeed < 0 ) LargeFulseSpeed = 0;
  if ( LargeFulseMaxCountPerHalfSec < 0 ) LargeFulseMaxCountPerHalfSec = 0;
  
  if ( LargePulseWidthInMs > 200 ) LargePulseWidthInMs = 200;
  if ( LargeFulseSpeed > 200 ) LargeFulseSpeed = 200;
  if ( LargeFulseMaxCountPerHalfSec > 20 ) LargeFulseMaxCountPerHalfSec = 20;
}

void DisplaySettings() {
  Serial.print("LeftStartLevel: ");      Serial.println(LeftStartLevel);
  Serial.print("RightStartLevel: ");     Serial.println(RightStartLevel);
  Serial.print("LargePulseWidthInMs: "); Serial.println(LargePulseWidthInMs);
  Serial.print("LargeFulseSpeed: ");     Serial.println(LargeFulseSpeed);
  Serial.print("LargeFulseMaxCountPerHalfSec: ");     Serial.println(LargeFulseMaxCountPerHalfSec);
}

String sliderValue = "0";

const char* PARAM_INPUT = "value";
const char* PARAM_SW = "sw";

// Create AsyncWebServer object on port 80
AsyncWebServer server(80);

const char index_html[] PROGMEM = R"rawliteral(
<!DOCTYPE HTML><html>
<head>
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>ESP Web Server</title>
  <style>
    html {font-family: Arial; display: inline-block; text-align: center;}
    h2 {font-size: 2.3rem;}
    p {font-size: 1.9rem;}
    body {max-width: 400px; margin:0px auto; padding-bottom: 25px;}
    .slider { -webkit-appearance: none; margin: 14px; width: 360px; height: 25px; background: #FFD65C;
      outline: none; -webkit-transition: .2s; transition: opacity .2s;}
    .slider::-webkit-slider-thumb {-webkit-appearance: none; appearance: none; width: 35px; height: 35px; background: #003249; cursor: pointer;}
    .slider::-moz-range-thumb { width: 35px; height: 35px; background: #003249; cursor: pointer; }
  </style>
</head>
<body>
  <h2>Pulse Width</h2>
  <p><span id="textSliderValue1">%SLIDERVALUE1%</span></p>
  <p><input type="range" onchange="updateSliderPWM(this, 1)" id="pwmSlider1" min="0" max="200" value="%SLIDERVALUE1%" step="1" class="slider"></p>

  <h2>PWM Duty</h2>
  <p><span id="textSliderValue2">%SLIDERVALUE2%</span></p>
  <p><input type="range" onchange="updateSliderPWM(this, 2)" id="pwmSlider2" min="0" max="200" value="%SLIDERVALUE2%" step="1" class="slider"></p>

  <h2>Repeats</h2>
  <p><span id="textSliderValue3">%SLIDERVALUE3%</span></p>
  <p><input type="range" onchange="updateSliderPWM(this, 3)" id="pwmSlider3" min="0" max="20" value="%SLIDERVALUE3%" step="1" class="slider"></p>
<script>
function updateSliderPWM(element, id) {
  var sliderValue = document.getElementById("pwmSlider" + id).value;
  document.getElementById("textSliderValue" + id).innerHTML = sliderValue;
  console.log(sliderValue);
  var xhr = new XMLHttpRequest();
  xhr.open("GET", "/slider?sw=" + id + "&value="+sliderValue, true);
  xhr.send();
}
</script>
</body>
</html>
)rawliteral";

// Replaces placeholder with button section in your web page
String processor(const String& var){
  //Serial.println(var);
  if (var == "SLIDERVALUE1"){
    return String(LargePulseWidthInMs);
  }
  if (var == "SLIDERVALUE2"){
    return String(LargeFulseSpeed);
  }
  if (var == "SLIDERVALUE3"){
    return String(LargeFulseMaxCountPerHalfSec);
  }
  return String();
}

void setup(){
  EEPROM.begin(100);
  
  pinMode(RightFrontFWD, OUTPUT);
  pinMode(RightFrontBWD, OUTPUT);
  pinMode(LeftFrontFWD, OUTPUT);
  pinMode(LeftFrontBWD, OUTPUT);
  // put your setup code here, to run once:
  Serial.begin(115200);      // make sure your Serial Monitor is also set at this baud rate.

  ReadSettingFromEEPROM();
  DisplaySettings();  

  serialdata[2] = '\0';

  LastOneSecTimeInMs = millis();

  // Connect to Wi-Fi
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi..");
  }

  // Print ESP Local IP Address
  Serial.println(WiFi.localIP());

  // Route for root / web page
  server.on("/", HTTP_GET, [](AsyncWebServerRequest *request){
    request->send_P(200, "text/html", index_html, processor);
  });

  // Send a GET request to <ESP_IP>/slider?value=<inputMessage>
  server.on("/slider", HTTP_GET, [] (AsyncWebServerRequest *request) {
    String inputMessage;
    String swMessage;
    // GET input1 value on <ESP_IP>/slider?value=<inputMessage>
    if (request->hasParam(PARAM_INPUT)) {
      inputMessage = request->getParam(PARAM_INPUT)->value();
      swMessage = request->getParam(PARAM_SW)->value();

      sliderValue = inputMessage;

      if ( swMessage == "1" ) {
        LargePulseWidthInMs = sliderValue.toInt();
        EEPROM.writeShort(8, LargePulseWidthInMs);
        EEPROM.commit();
        // Serial.print("Set LargePulseWidthInMs to "); Serial.println(LargePulseWidthInMs);
        // Serial.print("Verify LargePulseWidthInMs is "); Serial.println(EEPROM.readShort(8));
      }

      if ( swMessage == "2" ) {
        LargeFulseSpeed = sliderValue.toInt();
        EEPROM.writeShort(12, LargeFulseSpeed);
        EEPROM.commit();
        // Serial.print("Set LargeFulseSpeed to "); Serial.println(LargeFulseSpeed);
        // Serial.print("Verify LargeFulseSpeed is "); Serial.println(EEPROM.readShort(12));
      }
      
      if ( swMessage == "3" ) {
        LargeFulseMaxCountPerHalfSec = sliderValue.toInt();
        EEPROM.writeShort(16, LargeFulseMaxCountPerHalfSec);
        EEPROM.commit();
        // Serial.print("Set LargeFulseMaxCountPerHalfSec to "); Serial.println(LargeFulseMaxCountPerHalfSec);
        // Serial.print("Verify LargeFulseMaxCountPerHalfSec is "); Serial.println(EEPROM.readShort(16));
      }
    }
    else {
      inputMessage = "No message sent";
    }
    Serial.println(inputMessage);
    request->send(200, "text/plain", "OK");
  });
  
  // Start server
  server.begin();
}
  
void loop() {

  unsigned long NowInMs = millis();
  short tmpShort;

  if ( NowInMs - LastOneSecTimeInMs > 1000 ) {
    LastOneSecTimeInMs = NowInMs;
//    Serial.print("L: Stp=");  Serial.print(RunStep_Left);       
//    Serial.print(" Cnt=");  Serial.print(PendingMoves_Left);       
//    Serial.print(" A=");  Serial.print(NowInMs - DelayStartTimeInMs_Left);       
//    Serial.print(" B=");  Serial.println(PulseGapTimeInMs_Left);       

//    Serial.print("R: Stp=");  Serial.print(RunStep_Right);       
//    Serial.print(" Cnt=");  Serial.print(PendingMoves_Right);       
//    Serial.print(" A=");  Serial.print(NowInMs - DelayStartTimeInMs_Right);       
//    Serial.print(" B=");  Serial.println(PulseGapTimeInMs_Right);       
  }

  switch( RunStep_Left ) {
    case 0: //  Idle
      if ( PendingMoves_Left != 0 ) {        
        RunStep_Left = 1;                     //  Start the pulse
        DelayStartTimeInMs_Left = NowInMs;
        moveLeft(LargeFulseSpeed, PendingMoves_Left);

        if ( abs(PendingMoves_Left) > LargeFulseMaxCountPerHalfSec )
          tmpShort = LargeFulseMaxCountPerHalfSec;
        else 
          tmpShort = abs(PendingMoves_Left);
          
        PulseGapTimeInMs_Left = (500-10) / tmpShort - LargePulseWidthInMs;

//        Serial.print("L: 0=>1 Cnt=");  Serial.print(PendingMoves_Left);
      }
      break;

    case 1: //  Wait for the pulse to stop
      if ( NowInMs - DelayStartTimeInMs_Left > LargePulseWidthInMs ) {
        RunStep_Left = 2;                     //  Start the idle wait gap
        DelayStartTimeInMs_Left = NowInMs;
        moveLeft(0, 0);
        if (PendingMoves_Left>0)
          PendingMoves_Left--;
        else 
          PendingMoves_Left++;        
//        Serial.print("L: 1=>2 Cnt=");  Serial.println(PendingMoves_Left);       
      }
      break;

    case 2: //  Wait for the wait gap to pass
      if ( NowInMs - DelayStartTimeInMs_Left > PulseGapTimeInMs_Left ) {
        if ( PendingMoves_Left != 0 ) {        
          RunStep_Left = 1;                     //  Start the pulse
          DelayStartTimeInMs_Left = NowInMs;
          moveLeft(LargeFulseSpeed, PendingMoves_Left);
        }
        else {
          RunStep_Left = 0;
        }
//        Serial.print("L: 2=>");    Serial.print(RunStep_Left); 
//        Serial.print(", Cnt="); Serial.println(PendingMoves_Left);       
      }    
      break;  
  }
  
  switch( RunStep_Right ) {
    case 0: //  Idle
      if ( PendingMoves_Right != 0 ) {        
        RunStep_Right = 1;                     //  Start the pulse
        DelayStartTimeInMs_Right = NowInMs;
        moveRight(LargeFulseSpeed, PendingMoves_Right);

        if ( abs(PendingMoves_Right) > LargeFulseMaxCountPerHalfSec )
          tmpShort = LargeFulseMaxCountPerHalfSec;
        else 
          tmpShort = abs(PendingMoves_Right);
          
        PulseGapTimeInMs_Right = (500-10) / tmpShort - LargePulseWidthInMs;

//        Serial.print("R: 0=>1 Cnt=");  Serial.print(PendingMoves_Right);
      }
      break;

    case 1: //  Wait for the pulse to stop
      if ( NowInMs - DelayStartTimeInMs_Right > LargePulseWidthInMs ) {
        RunStep_Right = 2;                     //  Start the idle wait gap
        DelayStartTimeInMs_Right = NowInMs;
        moveRight(0, 0);
        if (PendingMoves_Right>0)
          PendingMoves_Right--;
        else 
          PendingMoves_Right++;        
//        Serial.print("R: 1=>2 Cnt=");  Serial.println(PendingMoves_Right);       
      }
      break;

    case 2: //  Wait for the wait gap to pass
      if ( NowInMs - DelayStartTimeInMs_Right > PulseGapTimeInMs_Right ) {
        if ( PendingMoves_Right != 0 ) {        
          RunStep_Right = 1;                     //  Start the pulse
          DelayStartTimeInMs_Right = NowInMs;
          moveRight(LargeFulseSpeed, PendingMoves_Right);
        }
        else {
          RunStep_Right = 0;
        }
//        Serial.print("R: 2=>");    Serial.print(RunStep_Right); 
//        Serial.print(", Cnt="); Serial.println(PendingMoves_Right);       
      }    
      break;  
  }
  
  ReadSerial0CommandInput();
  
/*  
  Serial.print('\t');
  int a = GamePad.getAngle();
  Serial.print("Angle: ");
  Serial.print(a);
  Serial.print('\t');
  int b = GamePad.getRadius();
  Serial.print("Radius: ");
  Serial.print(b);
  Serial.print('\t');
  float c = GamePad.getXaxisData();
  Serial.print("x_axis: ");
  Serial.print(c);
  Serial.print('\t');
  float d = GamePad.getYaxisData();
  Serial.print("y_axis: ");
  Serial.println(d);
  Serial.println();
*/  
}

void moveLeft(short myMotorSpeed, short Direction) {
  if ( Direction > 0 ) {
    analogWrite(LeftFrontFWD, myMotorSpeed);
    analogWrite(LeftFrontBWD, 0);    
  }
  else if ( Direction < 0 ) {
    analogWrite(LeftFrontFWD, 0);
    analogWrite(LeftFrontBWD, myMotorSpeed);    
  }
  else {
    analogWrite(LeftFrontBWD, 0);    
    analogWrite(LeftFrontFWD, 0);    
  }
}

void moveRight(short myMotorSpeed, short Direction ) {
  if ( Direction > 0 ) {
    analogWrite(RightFrontFWD, myMotorSpeed);
    analogWrite(RightFrontBWD, 0);    
  }
  else if ( Direction < 0 ) {
    analogWrite(RightFrontFWD, 0);
    analogWrite(RightFrontBWD, myMotorSpeed);        
  }  
  else {
    analogWrite(RightFrontBWD, 0);    
    analogWrite(RightFrontFWD, 0);    
  }
}


void moveForward(short myMotorSpeed) {
  analogWrite(RightFrontFWD, myMotorSpeed);
  analogWrite(RightFrontBWD, 0);
  analogWrite(LeftFrontFWD, myMotorSpeed);
  analogWrite(LeftFrontBWD, 0);
}

void moveBackward() {
  analogWrite(RightFrontFWD, 0);
  analogWrite(RightFrontBWD, motorspeed);
  analogWrite(LeftFrontFWD, 0);
  analogWrite(LeftFrontBWD, motorspeed);
}

void rotateRight() {
  analogWrite(RightFrontFWD, 0);
  analogWrite(RightFrontBWD, motorspeed);
  analogWrite(LeftFrontFWD, motorspeed);
  analogWrite(LeftFrontBWD, 0);
}

void rotateLeft() {
  analogWrite(RightFrontFWD, motorspeed);
  analogWrite(RightFrontBWD, 0);
  analogWrite(LeftFrontFWD, 0);
  analogWrite(LeftFrontBWD, motorspeed);
}

void stopMoving() {
  analogWrite(RightFrontFWD, 0);
  analogWrite(RightFrontBWD, 0);
  analogWrite(LeftFrontFWD, 0);
  analogWrite(LeftFrontBWD, 0);
}


void ReadSerial0CommandInput(){
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    
//    Serial.print(inChar, HEX);
//    digitalWrite(LED_BUILTIN, !digitalRead(LED_BUILTIN));   // turn the LED on (HIGH is the voltage level)  

    // add it to the inputString:
    Serial0InputString += inChar;
    // if the incoming character is a newline, set a flag so the main loop can
    // do something about it:
    if (inChar == '\n' || inChar == 0x0A) {
      Serial0InputComplete = true;
//      Serial.print("A:");
//      Serial.print(Serial0InputString);
    }
  }

  if ( Serial0InputComplete ) {
    Serial0InputComplete = false;

    int inputlen = Serial0InputString.length();
   
    char c1 = Serial0InputString[0];
    char c2;
    String strValue;
    switch ( c1 )
    {
      case 'S':
        if ( inputlen <= 2 ) {
          Serial0InputString = "";
          return;
        }
        
        c2 = Serial0InputString[1];
        strValue = Serial0InputString.substring(2, inputlen);
        
        switch ( c2 ) {
          case 'a':
            LeftStartLevel = strValue.toInt();
            EEPROM.writeShort(0, LeftStartLevel);
            EEPROM.commit();
            Serial.print("Set LeftStartLevel to "); Serial.println(LeftStartLevel);
            Serial.print("Verify LeftStartLevel is "); Serial.println(EEPROM.readShort(0));
            break;

          case 'b':
            RightStartLevel = strValue.toInt();
            EEPROM.writeShort(4, RightStartLevel);
            EEPROM.commit();
            Serial.print("Set RightStartLevel to "); Serial.println(RightStartLevel);
            Serial.print("Verify RightStartLevel is "); Serial.println(EEPROM.readShort(4));
            break;

          case 'c':
            LargePulseWidthInMs = strValue.toInt();
            EEPROM.writeShort(8, LargePulseWidthInMs);
            EEPROM.commit();
            Serial.print("Set LargePulseWidthInMs to "); Serial.println(LargePulseWidthInMs);
            Serial.print("Verify LargePulseWidthInMs is "); Serial.println(EEPROM.readShort(8));
            break;

          case 'd':
            LargeFulseSpeed = strValue.toInt();
            EEPROM.writeShort(12, LargeFulseSpeed);
            EEPROM.commit();
            Serial.print("Set LargeFulseSpeed to "); Serial.println(LargeFulseSpeed);
            Serial.print("Verify LargeFulseSpeed is "); Serial.println(EEPROM.readShort(12));
            
            break;
        }
        break;
      
      case 'D':
        if ( inputlen <= 4 ) {
          Serial0InputString = "";
          return;
        }
        serialdata[0] = Serial0InputString[1];
        serialdata[1] = Serial0InputString[2];
        short valLeft  = (short) strtol(serialdata, 0, 16) - 128;
        
        serialdata[0] = Serial0InputString[3];
        serialdata[1] = Serial0InputString[4];
        short valRight = (short) strtol(serialdata, 0, 16) - 128;

        PendingMoves_Left  = valLeft;
        PendingMoves_Right = valRight;

/*
        if ( valLeft < 127 ) {
          valLeft = 128 - valLeft + LeftStartLevel;
          if ( valLeft > 255 ) valLeft = 255;
          if ( valLeft < 0 ) valLeft = 0;
          analogWrite(LeftFrontFWD, 0);
          analogWrite(LeftFrontBWD, valLeft);          
        } 
        else if ( valLeft > 129 ) {
          valLeft = valLeft - 128 + LeftStartLevel;
          if ( valLeft > 255 ) valLeft = 255;
          if ( valLeft < 0 ) valLeft = 0;
          analogWrite(LeftFrontFWD, valLeft);
          analogWrite(LeftFrontBWD, 0);          
        }
        else {
          analogWrite(LeftFrontFWD, 0);
          analogWrite(LeftFrontBWD, 0);          
        }
        

        if ( valRight < 127 ) {
          valRight = 128 - valRight + RightStartLevel;
          if ( valRight > 255 ) valRight = 255;
          if ( valRight < 0 ) valRight = 0;
          analogWrite(RightFrontFWD, 0);
          analogWrite(RightFrontBWD, valRight);          
        }
        else if ( valRight > 129 ) {
          valRight = valRight - 128 + RightStartLevel;
          if ( valRight > 255 ) valRight = 255;
          if ( valRight < 0 ) valRight = 0;
          analogWrite(RightFrontFWD, valRight);
          analogWrite(RightFrontBWD, 0);          
        }
        else {
          analogWrite(RightFrontFWD, 0);
          analogWrite(RightFrontBWD, 0);          
        }
*/        
        break;
    }  
    Serial0InputString = "";
  }
}
