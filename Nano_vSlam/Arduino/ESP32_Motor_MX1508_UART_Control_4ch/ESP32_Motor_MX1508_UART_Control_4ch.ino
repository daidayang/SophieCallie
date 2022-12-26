//  https://www.instructables.com/ESP32-Mecanum-Wheels-Robot-and-Bluetooth-Gamepad-C/

#define CUSTOM_SETTINGS
// #define INCLUDE_GAMEPAD_MODULE
// #include <DabbleESP32.h>

#include <Arduino.h>

#define RightFrontBWD 12  //  INT4
#define RightFrontFWD 13  //  INT3
#define RightBackBWD  27  //  INT2
#define RightBackFWD  26  //  INT1

#define LeftFrontBWD  25  //  INT4
#define LeftFrontFWD  33  //  INT3
#define LeftBackBWD   32  //  INT2
#define LeftBackFWD   23  //  INT1

#define LeftStartLevel  104
#define RightStartLevel 104

// #define led_pin 33

int motorspeed = 128;

int v_left_front_Old;
int v_left_back_Old;
int v_right_front_Old;
int v_right_back_Old;

int v_left_front;
int v_left_back;
int v_right_front;
int v_right_back;

unsigned long LastSpeedChangeTimeInMs = 0;
unsigned long LastSpeedCommandReceivedInMs = 0;
String Serial1InputString = "";          // a String to hold incoming data
char Serial1InputBytes[20];
byte Serial1InputPos = 0;
bool Serial1InputComplete = false;     // whether the string is complete

#define RXD2 21     //  Color: Blue    ESP32 Pin 21   Jetson Nano Pin 8  
#define TXD2 22     //  Color: White   ESP32 Pin 22   Jetson Nano Pin 10

// #define RXD2 16     //  Color: Blue    ESP32 Pin 21   Jetson Nano Pin 8 
// #define TXD2 17     //  Color: White   ESP32 Pin 22   Jetson Nano Pin 10


void setup() {
  // pinMode(led_pin, OUTPUT);
  // pinMode(LED_BUILTIN, OUTPUT);

  pinMode(RightFrontFWD, OUTPUT);
  analogWrite(RightFrontFWD, 0);

  pinMode(RightFrontBWD, OUTPUT);
  analogWrite(RightFrontBWD, 0);

  pinMode(RightBackFWD, OUTPUT);
  analogWrite(RightBackFWD, 0);

  pinMode(RightBackBWD, OUTPUT);
  analogWrite(RightBackBWD, 0);

  pinMode(LeftFrontFWD, OUTPUT);
  analogWrite(LeftFrontFWD, 0);

  pinMode(LeftFrontBWD, OUTPUT);
  analogWrite(LeftFrontBWD, 0);

  pinMode(LeftBackFWD, OUTPUT);
  analogWrite(LeftBackFWD, 0);

  pinMode(LeftBackBWD, OUTPUT);
  analogWrite(LeftBackBWD, 0);

  // put your setup code here, to run once:
  Serial.begin(115200);      // make sure your Serial Monitor is also set at this baud rate.
  Serial1.begin(115200, SERIAL_8N1, RXD2, TXD2);

//  Dabble.begin("MyEsp32");       //set bluetooth name of your device

  LastSpeedChangeTimeInMs = millis();
  LastSpeedCommandReceivedInMs = LastSpeedChangeTimeInMs;
}

void loop() {
  unsigned long NowInMs = millis();

  ReadSerial0CommandInput();

  if ( (NowInMs - LastSpeedCommandReceivedInMs) > 5000 ) {
    v_left_front = 0;
    v_left_back = 0;
    v_right_front = 0;
    v_right_back = 0;    
  }

  if ( v_left_front != v_left_front_Old ) {
    v_left_front_Old = v_left_front;

    if ( v_left_front > 0 ) {
      analogWrite(LeftFrontFWD, v_left_front);
      analogWrite(LeftFrontBWD, 0);      
    }
    else {
      analogWrite(LeftFrontFWD, 0);
      analogWrite(LeftFrontBWD, -v_left_front);      
    }    
  }

  if ( v_left_back != v_left_back_Old ) {
    v_left_back_Old = v_left_back;

    if ( v_left_back > 0 ) {
      analogWrite(LeftBackFWD, v_left_back);
      analogWrite(LeftBackBWD, 0);      
    }
    else {
      analogWrite(LeftBackFWD, 0);
      analogWrite(LeftBackBWD, -v_left_back);      
    }
  }
  
  if ( v_right_front != v_right_front_Old ) {
    v_right_front_Old = v_right_front;

    if ( v_right_front > 0 ) {
      analogWrite(RightFrontFWD, v_right_front);
      analogWrite(RightFrontBWD, 0);      
    }
    else {
      analogWrite(RightFrontFWD, 0);
      analogWrite(RightFrontBWD, -v_right_front);      
    }
  }

  if ( v_right_back != v_right_back_Old ) {
    v_right_back_Old = v_right_back;

    if ( v_right_back > 0 ) {
      analogWrite(RightBackFWD, v_right_back);
      analogWrite(RightBackBWD, 0);      
    }
    else {
      analogWrite(RightBackFWD, 0);
      analogWrite(RightBackBWD, -v_right_back);      
    }
  }

/*
  analogWrite(RightFrontFWD, motorspeed);
  analogWrite(RightFrontBWD, 0);
  delay(6000);
  
  analogWrite(RightFrontFWD, 0);
  analogWrite(RightFrontBWD, motorspeed);
  delay(3000);
  
  analogWrite(RightFrontBWD, 0);
  delay(1000);

  analogWrite(RightBackFWD, motorspeed);
  analogWrite(RightBackBWD, 0);
  delay(6000);

  analogWrite(RightBackFWD, 0);
  analogWrite(RightBackBWD, motorspeed);
  delay(3000);

  analogWrite(RightBackBWD, 0);
  delay(1000);

  analogWrite(LeftFrontFWD, motorspeed);
  analogWrite(LeftFrontBWD, 0);
  delay(6000);

  analogWrite(LeftFrontFWD, 0);
  analogWrite(LeftFrontBWD, motorspeed);
  delay(3000);

  analogWrite(LeftFrontBWD, 0);
  delay(1000);

  analogWrite(LeftBackFWD, motorspeed);
  analogWrite(LeftBackBWD, 0);
  delay(6000);

  analogWrite(LeftBackFWD, 0);
  analogWrite(LeftBackBWD, motorspeed);
  delay(3000);

  analogWrite(LeftBackBWD, 0);
  delay(1000);

  return;
*/
}

void moveForward() {
  analogWrite(RightFrontFWD, motorspeed);
  analogWrite(RightFrontBWD, 0);
  analogWrite(RightBackFWD, motorspeed);
  analogWrite(RightBackBWD, 0);

  analogWrite(LeftFrontFWD, motorspeed);
  analogWrite(LeftFrontBWD, 0);
  analogWrite(LeftBackFWD, motorspeed);
  analogWrite(LeftBackBWD, 0);
}

void moveBackward() {
  analogWrite(RightFrontFWD, 0);
  analogWrite(RightFrontBWD, motorspeed);
  analogWrite(RightBackFWD, 0);
  analogWrite(RightBackBWD, motorspeed);

  analogWrite(LeftFrontFWD, 0);
  analogWrite(LeftFrontBWD, motorspeed);
  analogWrite(LeftBackFWD, 0);
  analogWrite(LeftBackBWD, motorspeed);
}

void rotateRight() {
  analogWrite(RightFrontFWD, 0);
  analogWrite(RightFrontBWD, motorspeed);
  analogWrite(RightBackFWD, 0);
  analogWrite(RightBackBWD, motorspeed);

  analogWrite(LeftFrontFWD, motorspeed);
  analogWrite(LeftFrontBWD, 0);
  analogWrite(LeftBackFWD, motorspeed);
  analogWrite(LeftBackBWD, 0);
}

void rotateLeft() {
  analogWrite(RightFrontFWD, motorspeed);
  analogWrite(RightFrontBWD, 0);
  analogWrite(RightBackFWD, motorspeed);
  analogWrite(RightBackBWD, 0);

  analogWrite(LeftFrontFWD, 0);
  analogWrite(LeftFrontBWD, motorspeed);
  analogWrite(LeftBackFWD, 0);
  analogWrite(LeftBackBWD, motorspeed);
}

void stopMoving() {
  analogWrite(RightFrontFWD, 0);
  analogWrite(RightFrontBWD, 0);
  analogWrite(RightBackFWD, 0);
  analogWrite(RightBackBWD, 0);

  analogWrite(LeftFrontFWD, 0);
  analogWrite(LeftFrontBWD, 0);
  analogWrite(LeftBackFWD, 0);
  analogWrite(LeftBackBWD, 0);
}

void moveSidewaysRight() {
  analogWrite(RightFrontFWD, 0);
  analogWrite(RightFrontBWD, motorspeed);
  analogWrite(RightBackFWD, motorspeed);
  analogWrite(RightBackBWD, 0);

  analogWrite(LeftFrontFWD, motorspeed);
  analogWrite(LeftFrontBWD, 0);
  analogWrite(LeftBackFWD, 0);
  analogWrite(LeftBackBWD, motorspeed);
}

void moveSidewaysLeft() {
  analogWrite(RightFrontFWD, motorspeed);
  analogWrite(RightFrontBWD, 0);
  analogWrite(RightBackFWD, 0);
  analogWrite(RightBackBWD, motorspeed);

  analogWrite(LeftFrontFWD, 0);
  analogWrite(LeftFrontBWD, motorspeed);
  analogWrite(LeftBackFWD, motorspeed);
  analogWrite(LeftBackBWD, 0);
}

void moveRightForward() {
  analogWrite(RightFrontFWD, 0);
  analogWrite(RightFrontBWD, 0);
  analogWrite(RightBackFWD, motorspeed);
  analogWrite(RightBackBWD, 0);

  analogWrite(LeftFrontFWD, motorspeed);
  analogWrite(LeftFrontBWD, 0);
  analogWrite(LeftBackFWD, 0);
  analogWrite(LeftBackBWD, 0);
}

void moveLeftForward() {
  analogWrite(RightFrontFWD, motorspeed);
  analogWrite(RightFrontBWD, 0);
  analogWrite(RightBackFWD, 0);
  analogWrite(RightBackBWD, 0);

  analogWrite(LeftFrontFWD, 0);
  analogWrite(LeftFrontBWD, 0);
  analogWrite(LeftBackFWD, motorspeed);
  analogWrite(LeftBackBWD, 0);
}


/*
 * Return value:  True  => a valid serial command has been received
 *                False => no valid serial command has been received yet. 
 */
void ReadSerial0CommandInput(){
  int TmpInt;
  
  while (Serial1.available()) {
    // get the new byte:
    char inChar = (char)Serial1.read();
    // Serial.print(inChar, HEX);
    // digitalWrite(LED_BUILTIN, !digitalRead(LED_BUILTIN));

    if ( inChar == 0x81 ) {
      Serial1InputComplete = false;
      Serial1InputPos = 0;
    }

    Serial1InputBytes[Serial1InputPos++] = inChar;

    if ( inChar == 0x80 ) {
      Serial1InputComplete = true;
    }
  }

  if ( Serial1InputComplete ) {
    Serial1InputComplete = false;

//    for(int idx=0; idx++; idx<Serial1InputPos ) {
//      Serial.print( (short) Serial1InputBytes[idx] );
//      Serial.print(" ");
//    }
//    Serial.println();

    int inputlen = Serial1InputPos;

//    Serial.print("inputString=");
//    Serial.println(Serial1InputString);
//    Serial.print("inputlen=");
//    Serial.println(inputlen);
    
    char c1 = Serial1InputBytes[0];
    char c2;
    switch ( c1 )
    {
      case '1':
        Serial.println("Steering SetZeroPos()");
        break;

      case '2':
        Serial.println("Steering ReadZeroPos()");
        break;

      case 0x81:
//        Serial.print("inputlen=");
//        Serial.println(inputlen);
        if ( inputlen != 10 ) {
          Serial1InputPos = 0;
          return;
        }

        // target value range -255 0 +255
        // actual value range 1 256 511
        
        c2 = Serial1InputBytes[1];
        TmpInt = c2 << 7;
        c2 = Serial1InputBytes[2];
        TmpInt += c2;
        v_left_front = TmpInt - 256;

        c2 = Serial1InputBytes[3];
        TmpInt = c2 << 7;
        c2 = Serial1InputBytes[4];
        TmpInt += c2;
        v_left_back = TmpInt - 256;

        c2 = Serial1InputBytes[5];
        TmpInt = c2 << 7;
        c2 = Serial1InputBytes[6];
        TmpInt += c2;
        v_right_front = TmpInt - 256;

        c2 = Serial1InputBytes[7];
        TmpInt = c2 << 7;
        c2 = Serial1InputBytes[8];
        TmpInt += c2;
        v_right_back = TmpInt - 256;

//        Serial.print(" lf=");
//        Serial.print(v_left_front);
//        Serial.print(" lb=");
//        Serial.print(v_left_back);
//        Serial.print(" rf=");
//        Serial.print(v_right_front);
//        Serial.print(" rb=");
//        Serial.println(v_right_back);

//        digitalWrite(led_pin, !digitalRead(led_pin));

        LastSpeedCommandReceivedInMs = millis();
        break;

      case 'S':
        if ( inputlen <= 2 ) {
          Serial1InputString = "";
          return;
        }
          
        c2 = Serial1InputString[1];
        String strValue = Serial1InputString.substring(2, inputlen);   
        int intValue = 0;
        switch( c2 ) {
          case 'B':
            intValue = strValue.toInt();
            Serial.print("SB = ");
            Serial.println(intValue);
          break;
          case 'A':
            intValue = strValue.toInt();
            Serial.print("SA = ");
            Serial.println(intValue);
          break;
        }
        break;
    }  
    Serial1InputString = "";
  }
}

void setMecanumDrive(double translationAngle, double translationPower, double turnPower)
{
  //  https://compendium.readthedocs.io/en/latest/tasks/drivetrains/mecanum.html
    // calculate motor power
    double ADPower = translationPower * sqrt(2) * 0.5 * (sin(translationAngle) + cos(translationAngle));
    double BCPower = translationPower * sqrt(2) * 0.5 * (sin(translationAngle) - cos(translationAngle));

    // check if turning power will interfere with normal translation
    // check ADPower to see if trying to apply turnPower would put motor power over 1.0 or under -1.0
    double turningScale = max(abs(ADPower + turnPower), abs(ADPower - turnPower));
    // check BCPower to see if trying to apply turnPower would put motor power over 1.0 or under -1.0
    turningScale = max(turningScale, max(abs(BCPower + turnPower), abs(BCPower - turnPower)));

    // adjust turn power scale correctly
    if (abs(turningScale) < 1.0)
    {
        turningScale = 1.0;
    }

    // set the motors, and divide them by turningScale to make sure none of them go over the top, which would alter the translation angle
    int LEFT_FRONT_DRIVE_MOTOR  = (ADPower - turningScale) / turningScale;
    int LEFT_BACK_DRIVE_MOTOR   = (BCPower - turningScale) / turningScale;
    int RIGHT_FRONT_DRIVE_MOTOR = (BCPower + turningScale) / turningScale;
    int RIGHT_BACK_DRIVE_MOTOR  = (ADPower + turningScale) / turningScale;
}
