//  https://www.instructables.com/ESP32-Mecanum-Wheels-Robot-and-Bluetooth-Gamepad-C/

#include <Arduino.h>

#define myLED1 27
#define myLED2 26
#define myLED3 25
#define myLED4 33 

unsigned long LastSpeedChangeTimeInMs = 0;
int CurVal_PWM = 0;

void setup() {
  pinMode(myLED1, OUTPUT);
  analogWrite(myLED1, 0);

  pinMode(myLED2, OUTPUT);
  analogWrite(myLED2, 0);

  pinMode(myLED3, OUTPUT);
  analogWrite(myLED3, 0);

  pinMode(myLED4, OUTPUT);
  analogWrite(myLED4, 0);

  // put your setup code here, to run once:
  Serial.begin(115200);      // make sure your Serial Monitor is also set at this baud rate.
  
  LastSpeedChangeTimeInMs = millis();
}

void loop() {
  unsigned NowInMs =  millis();
  int tmpVal_PWM;

  if ( NowInMs - LastSpeedChangeTimeInMs > 50 ) {
    LastSpeedChangeTimeInMs = NowInMs;
    CurVal_PWM += 8;

    if ( CurVal_PWM > 255 )
      CurVal_PWM = 0;
      
    analogWrite(myLED1, CurVal_PWM);

    tmpVal_PWM = CurVal_PWM + 64;
    if ( tmpVal_PWM > 255 )
      tmpVal_PWM -= 256;
    analogWrite(myLED2, tmpVal_PWM);
    
    tmpVal_PWM = CurVal_PWM + 128;
    if ( tmpVal_PWM > 255 )
      tmpVal_PWM -= 256;
    analogWrite(myLED3, tmpVal_PWM);
    
    tmpVal_PWM = CurVal_PWM + 192;
    if ( tmpVal_PWM > 255 )
      tmpVal_PWM -= 256;
    analogWrite(myLED4, tmpVal_PWM);
  }
}
