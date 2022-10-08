#!/usr/bin/env python
import time
import serial
import math
from math import trunc
import rospy
from geometry_msgs.msg import Twist

class MotorDriverUART:

    # configure the serial connections (the parameters differs on the device you are connecting to)
    ser = serial.Serial(
        port='/dev/ttyUSB0',
        baudrate=115200,
        parity=serial.PARITY_NONE,
        stopbits=serial.STOPBITS_ONE,
        bytesize=serial.EIGHTBITS
    )
    ser.isOpen()

    past_x = -999
    past_y = -999

    def __init__(self):
        rospy.init_node('motor_driver', anonymous=True, log_level=rospy.DEBUG)
        node_name = rospy.get_name()

        rospy.loginfo("MotorDriverUART.init")

        rospy.Subscriber("/drive_cmd", Twist, self.twist_callback)
        rospy.on_shutdown(self.shutdown_cb)

    def twist_callback(self, data):

        if ( data.linear.x != self.past_x or data.angular.z != self.past_y):

            # rospy.loginfo(rospy.get_caller_id() + \
            #          ": Linear.x: %f -- Angular.z: %f", \
            #          data.linear.x, data.angular.z)

            self.past_x = data.linear.x
            self.past_y = data.angular.z

            turn = 0
            if abs(self.past_y) > 30:
                if self.past_y > 0:
                    turn = 9 + trunc(self.past_y / 30)
                else:
                    turn = -9 - trunc(self.past_y / 30)

            move = 0
            if abs(self.past_x) > 5:
                if self.past_x > 0:
                    move = 24
                else:
                    move = -24
            move = 0
            strhex = 'D{:02x}{:02x}'.format(move+turn+128,move-turn+128)

            # rospy.loginfo(rospy.get_caller_id() + ": left: %f -- right: %f", move+turn, move-turn)

            if self.ser.isOpen():
                self.ser.write(strhex + "\r\n")
                rospy.loginfo(strhex)
                # self.ser.write("D8080")

    def shutdown_cb(self):
        rospy.loginfo(rospy.get_caller_id() + ": Shutdown callback")
        self.ser.write('D8080\r\n')
        rospy.loginfo(rospy.get_caller_id() + ": Stop motor")
        self.ser.close()

def main():
    mdriver = MotorDriverUART()
    rate = rospy.Rate(1)

    while not rospy.is_shutdown():
        rate.sleep()

if __name__ == '__main__':
    main()
