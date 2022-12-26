#!/usr/bin/python3
import time
import serial

print("UART Demonstration Program")


serial_port = serial.Serial(
    port="/dev/ttyTHS1",
    baudrate=115200,
    bytesize=serial.EIGHTBITS,
    parity=serial.PARITY_NONE,
    stopbits=serial.STOPBITS_ONE,
)
# Wait a second to let the port initialize
time.sleep(1)

try:
    # Send a simple header
    serial_port.write("UART motor test\r\n".encode())
    speed = 100;

    while True:
        time.sleep(0.01)

        speed = speed & 0X3FFF
        msb = speed >> 7
        lsb = speed & 0X007F

        packet = bytearray()
        packet.append(0x81)
        packet.append(msb)
        packet.append(lsb)
        packet.append(msb)
        packet.append(lsb)
        packet.append(msb)
        packet.append(lsb)
        packet.append(msb)
        packet.append(lsb)
        packet.append(0x80)

        serial_port.write(packet)
        print( speed )
        speed = speed + 1

        if serial_port.inWaiting() > 0:
            data = serial_port.read()
            print(data)
            serial_port.write(data)
            # if we get a carriage return, add a line feed too
            # \r is a carriage return; \n is a line feed
            # This is to help the tty program on the other end 
            # Windows is \r\n for carriage return, line feed
            # Macintosh and Linux use \n
            if data == "\r".encode():
                # For Windows boxen on the other end
                serial_port.write("\n".encode())


except KeyboardInterrupt:
    print("Exiting Program")

except Exception as exception_error:
    print("Error occurred. Exiting Program")
    print("Error: " + str(exception_error))

finally:
    serial_port.close()
    pass
