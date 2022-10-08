#include <EEPROM.h>

#define CUSTOM_SETTINGS

#include <Arduino.h>

#define RightFrontFWD 27
#define RightFrontBWD 26
#define LeftFrontFWD  14
#define LeftFrontBWD  12

short LeftStartLevel =  100;
short RightStartLevel = 104;

int motorspeed = 64;
int incomingByte = 0;    // for incoming serial data

String Serial0InputString = "";         // a String to hold incoming data
bool   Serial0InputComplete = false;     // whether the string is complete

char serialdata[3];


void ReadSettingFromEEPROM() {
  LeftStartLevel = EEPROM.readShort(0);
  RightStartLevel = EEPROM.readShort(4);
}

void DisplaySettings() {
  Serial.print("LeftStartLevel: "); Serial.println(LeftStartLevel);
  Serial.print("RightStartLevel: "); Serial.println(RightStartLevel);
}

void setup() {
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
}

void loop() {
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

void moveForward() {
  analogWrite(RightFrontFWD, motorspeed);
  analogWrite(RightFrontBWD, 0);
  analogWrite(LeftFrontFWD, motorspeed);
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
        }
        break;
      
      case 'D':
        if ( inputlen <= 4 ) {
          Serial0InputString = "";
          return;
        }
        serialdata[0] = Serial0InputString[1];
        serialdata[1] = Serial0InputString[2];
        short valLeft = (short) strtol(serialdata, 0, 16);
        
        serialdata[0] = Serial0InputString[3];
        serialdata[1] = Serial0InputString[4];
        short valRight = (short) strtol(serialdata, 0, 16);

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
        break;
    }  
    Serial0InputString = "";
  }
}
