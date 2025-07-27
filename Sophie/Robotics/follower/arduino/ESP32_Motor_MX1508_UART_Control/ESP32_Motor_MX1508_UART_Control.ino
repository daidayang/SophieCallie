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

String Serial0InputString   = "";        // a String to hold incoming data
bool   Serial0InputComplete = false;     // whether the string is complete

char serialdata[3];

unsigned long LastOneSecTimeInMs;
short FinePulseWidthInMs = 50;
short FineFulseSpeed     = 100;

short PendingMoves_Left  = 0;
short PendingMoves_Right = 0;

short RunStep_Left  = 0;
short RunStep_Right = 0;
unsigned long DelayStartTimeInMs_Left;
unsigned long DelayStartTimeInMs_Right;

int PulseGapTimeInMs_Left;
int PulseGapTimeInMs_Right;

void ReadSettingFromEEPROM() {
  LeftStartLevel = EEPROM.readShort(0);
  RightStartLevel = EEPROM.readShort(4);
  FinePulseWidthInMs = EEPROM.readShort(8);
  FineFulseSpeed = EEPROM.readShort(12);
}

void DisplaySettings() {
  Serial.print("LeftStartLevel: ");     Serial.println(LeftStartLevel);
  Serial.print("RightStartLevel: ");    Serial.println(RightStartLevel);
  Serial.print("FinePulseWidthInMs: "); Serial.println(FinePulseWidthInMs);
  Serial.print("FineFulseSpeed: ");     Serial.println(FineFulseSpeed);
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

  LastOneSecTimeInMs = millis();
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
        moveLeft(FineFulseSpeed, PendingMoves_Left);

        if ( abs(PendingMoves_Left) > 8 )
          tmpShort = 8;
        else 
          tmpShort = abs(PendingMoves_Left);
          
        PulseGapTimeInMs_Left = (500-50) / tmpShort - FinePulseWidthInMs;

//        Serial.print("L: 0=>1 Cnt=");  Serial.print(PendingMoves_Left);
      }
      break;

    case 1: //  Wait for the pulse to stop
      if ( NowInMs - DelayStartTimeInMs_Left > FinePulseWidthInMs ) {
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
          moveLeft(FineFulseSpeed, PendingMoves_Left);
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
        moveRight(FineFulseSpeed, PendingMoves_Right);

        if ( abs(PendingMoves_Right) > 8 )
          tmpShort = 8;
        else 
          tmpShort = abs(PendingMoves_Right);
          
        PulseGapTimeInMs_Right = (500-50) / tmpShort - FinePulseWidthInMs;

//        Serial.print("R: 0=>1 Cnt=");  Serial.print(PendingMoves_Right);
      }
      break;

    case 1: //  Wait for the pulse to stop
      if ( NowInMs - DelayStartTimeInMs_Right > FinePulseWidthInMs ) {
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
          moveRight(FineFulseSpeed, PendingMoves_Right);
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
            FinePulseWidthInMs = strValue.toInt();
            EEPROM.writeShort(8, FinePulseWidthInMs);
            EEPROM.commit();
            Serial.print("Set FinePulseWidthInMs to "); Serial.println(FinePulseWidthInMs);
            Serial.print("Verify FinePulseWidthInMs is "); Serial.println(EEPROM.readShort(8));
            break;

          case 'd':
            FineFulseSpeed = strValue.toInt();
            EEPROM.writeShort(12, FineFulseSpeed);
            EEPROM.commit();
            Serial.print("Set FineFulseSpeed to "); Serial.println(FineFulseSpeed);
            Serial.print("Verify FineFulseSpeed is "); Serial.println(EEPROM.readShort(12));
            
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
