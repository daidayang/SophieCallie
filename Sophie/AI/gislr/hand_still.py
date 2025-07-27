import cv2
import mediapipe as mp
import numpy as np
import time

# Initialize MediaPipe Hands
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(min_detection_confidence=0.5, min_tracking_confidence=0.5)
mp_drawing = mp.solutions.drawing_utils

# Helper function to calculate the Euclidean distance
def calculate_distance(landmark1, landmark2):
    return np.sqrt((landmark1.x - landmark2.x)**2 +
                   (landmark1.y - landmark2.y)**2 +
                   (landmark1.z - landmark2.z)**2)

# Function to get hand name
def get_hand_name(results, index):
    """Get the hand name (Left or Right) based on its index."""
    if results.multi_handedness and len(results.multi_handedness) > index:
        return results.multi_handedness[index].classification[0].label
    return f"Hand {index + 1}"

# Constants
PAUSE_THRESHOLD = 0.025  # 1mm in normalized space
PAUSE_DURATION = 0.25  # 0.25 seconds

# Variables
previous_landmarks = None
pause_start_time = None
hands_paused = False

# Open the webcam
cap = cv2.VideoCapture(0)

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break

    # Flip the frame horizontally for a selfie-view display
    frame = cv2.flip(frame, 1)
    # Convert the color space from BGR to RGB
    frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

    # Process the frame and detect hand landmarks
    results = hands.process(frame_rgb)

    # Extract hand landmarks
    if results.multi_hand_landmarks:
        all_hands_paused = True

        # Process each hand and identify by its index
        for hand_index, hand_landmarks in enumerate(results.multi_hand_landmarks):
            hand_name = get_hand_name(results, hand_index)  # Get the hand name
            print(f"Calculating movement for {hand_name}...")  # Debug output

            if previous_landmarks is not None and len(previous_landmarks) > hand_index:
                # Calculate movement for each landmark
                movements = [
                    calculate_distance(prev, curr)
                    for prev, curr in zip(previous_landmarks[hand_index], hand_landmarks.landmark)
                ]
                max_movement = max(movements)

                # Debug output for movement
                print(f"{hand_name} - Max Movement: {max_movement}")

                # Check if the hand has paused
                if max_movement >= PAUSE_THRESHOLD:
                    all_hands_paused = False

            # Draw hand landmarks on the frame
            mp_drawing.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)

        # Update the previous landmarks for all hands
        previous_landmarks = [
            [lm for lm in hand_landmarks.landmark] for hand_landmarks in results.multi_hand_landmarks
        ]

        if all_hands_paused:
            if pause_start_time is None:
                pause_start_time = time.time()
            elif time.time() - pause_start_time >= PAUSE_DURATION:
                hands_paused = True
        else:
            pause_start_time = None
            hands_paused = False

    else:
        previous_landmarks = None
        pause_start_time = None
        hands_paused = False

    # Display status
    if hands_paused:
        cv2.putText(frame, "Hands Paused!", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)
    else:
        cv2.putText(frame, "Hands Moving!", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)

    # Display the frame
    cv2.imshow('Hand Tracking', frame)

    # Break the loop if 'q' is pressed
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release resources
cap.release()
cv2.destroyAllWindows()
