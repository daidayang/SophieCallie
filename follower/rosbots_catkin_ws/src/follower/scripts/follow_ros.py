#!/usr/bin/env python
import cv2
import math
import rospy
from sensor_msgs.msg import Image
from cv_bridge import CvBridge, CvBridgeError
from std_msgs.msg import UInt16
from geometry_msgs.msg import Twist
from picamera import PiCamera
import numpy as np

def main():
    camera = PiCamera(resolution=(320, 240), framerate=30)
    # Set ISO to the desired value
    camera.iso = 100
    # Wait for the automatic gain control to settle
    rospy.sleep(2)
    # sleep(2)
    # Now fix the values
    camera.shutter_speed = camera.exposure_speed
    camera.exposure_mode = 'off'
    g = camera.awb_gains
    camera.awb_mode = 'off'
    camera.awb_gains = g

    rospy.init_node('follower', anonymous=False, log_level=rospy.DEBUG)

    # front_pub = rospy.Publisher('/image_front', Image, queue_size=1)
    drive_cmd_pub = rospy.Publisher('/drive_cmd', Twist, queue_size=1)

    # bridge = CvBridge()
    while not rospy.is_shutdown():

        try:
            image = np.empty((240 * 320 * 3,), dtype=np.uint8)
            camera.capture(image, 'bgr', use_video_port=True)
            image = image.reshape((240, 320, 3))
            # rospy.logdebug("runs")
            if True:
                # Convert BGR to HSV
                hsv = cv2.cvtColor(image, cv2.COLOR_BGR2HSV)
                # define range of red color in HSV
                lower_red = np.array([161, 155, 84])
                upper_red = np.array([179, 255, 255])

                mask = cv2.inRange (hsv, lower_red, upper_red)
                contours = cv2.findContours(mask.copy(), cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)[-2]
                if len(contours) > 0:
                    red_area = max(contours, key=cv2.contourArea)
                    x, y, w, h = cv2.boundingRect(red_area)
                    # rospy.loginfo("x=%d, w=%d", x, w)

                    twist_msg = Twist()

                    twist_msg.linear.x = 30 - w
                    twist_msg.angular.z = x - 160

                    drive_cmd_pub.publish(twist_msg)

                    rospy.loginfo("x=%d", x);
                    # x = x * 255 / 319;
                    # strhex = 'D{:02x}{:02x}'.format(x,x)
                    # rospy.logdebug(strhex);
                    # ser.write

        except KeyboardInterrupt:
            rospy.logerror("err")
            break

if __name__=='__main__':
    main()
