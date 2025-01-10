import cv2
import mediapipe as mp
import pandas as pd
import numpy as np
import time
import tflite_runtime.interpreter as tflite
import os


# Variables
last_frame_save_time = None

# Initialize the TensorFlow Lite interpreter
interpreter = tflite.Interpreter("model.tflite")
found_signatures = list(interpreter.get_signature_list().keys())
pred_fn = interpreter.get_signature_runner("serving_default")

# Load training data
train = pd.read_csv("train.csv")
train['sign_ord'] = train['sign'].astype('category').cat.codes
SIGN2ORD = train[['sign', 'sign_ord']].set_index('sign').squeeze().to_dict()
ORD2SIGN = train[['sign_ord', 'sign']].set_index('sign_ord').squeeze().to_dict()

# Initialize MediaPipe holistic model
mp_drawing = mp.solutions.drawing_utils
mp_drawing_styles = mp.solutions.drawing_styles
mp_holistic = mp.solutions.holistic


def load_relevant_data_subset(pq_path):
    data_columns = ['x', 'y', 'z']
    data = pd.read_parquet(pq_path, columns=data_columns)
    n_frames = int(len(data) / ROWS_PER_FRAME)
    print( f"data len: {len(data)}, frames: {n_frames}")
    data = data.values.reshape(n_frames, ROWS_PER_FRAME, len(data_columns))
    return data.astype(np.float32)

# Helper function to calculate the Euclidean distance
def calculate_distance(landmark1, landmark2):
    # print( f"landmark2.len: {len(landmark2)}" )
    # print( landmark2 )
    # return 1.0

    if hasattr(landmark1, 'x') and hasattr(landmark2, 'x'):
        return np.sqrt((landmark1.x - landmark2.x)**2 +
                        (landmark1.y - landmark2.y)**2 +
                        (landmark1.z - landmark2.z)**2)
    return -1

def create_frame_landmark_df(results, frame, xyz, hands_paused):

    if not hasattr( create_frame_landmark_df, "pause_start_time"):
        create_frame_landmark_df.previous_left_hand_landmarks = None
        create_frame_landmark_df.previous_right_hand_landmarks = None
        create_frame_landmark_df.pause_start_time = None
        create_frame_landmark_df.PAUSE_THRESHOLD = 0.025  # 2.5cm in normalized space
        create_frame_landmark_df.PAUSE_DURATION = 0.25  # 0.25 seconds

    hands_staddy_paused = hands_paused
    max_movement_left = -1.0
    max_movement_right = -1.0
    all_hands_paused = True

    xyz_skel = (xyz[["type", "landmark_index"]].drop_duplicates().reset_index(drop=True).copy())
    face = pd.DataFrame()
    pose = pd.DataFrame()
    left_hand = pd.DataFrame()
    right_hand = pd.DataFrame()

    if results.face_landmarks:
        for i, point in enumerate(results.face_landmarks.landmark):
            face.loc[i, ["x", "y", "z"]] = [point.x, point.y, point.z]
    if results.pose_landmarks:
        for i, point in enumerate(results.pose_landmarks.landmark):
            pose.loc[i, ["x", "y", "z"]] = [point.x, point.y, point.z]

    if results.left_hand_landmarks:
        # print(results.left_hand_landmarks.landmark)

        if create_frame_landmark_df.previous_left_hand_landmarks is not None:
            movements_left_hand = [
                calculate_distance(prev, curr)
                for prev, curr in zip(create_frame_landmark_df.previous_left_hand_landmarks, results.left_hand_landmarks.landmark)
            ]
            max_movement_left = max(movements_left_hand)

        # print(f"left max: {max_movement_left}")
        create_frame_landmark_df.previous_left_hand_landmarks = results.left_hand_landmarks.landmark

        for i, point in enumerate(results.left_hand_landmarks.landmark):
            left_hand.loc[i, ["x", "y", "z"]] = [point.x, point.y, point.z]

    if results.right_hand_landmarks:
        # print(results.right_hand_landmarks.landmark)

        if create_frame_landmark_df.previous_right_hand_landmarks is not None:
            movements_right_hand = [
                calculate_distance(prev, curr)
                for prev, curr in zip(create_frame_landmark_df.previous_right_hand_landmarks, results.right_hand_landmarks.landmark)
            ]
            max_movement_right = max(movements_right_hand)

        # print(f"right max: {max_movement_right}")
        create_frame_landmark_df.previous_right_hand_landmarks = results.right_hand_landmarks.landmark

        for i, point in enumerate(results.right_hand_landmarks.landmark):
            right_hand.loc[i, ["x", "y", "z"]] = [point.x, point.y, point.z]


    face = face.reset_index().rename(columns={"index": "landmark_index"}).assign(type="face")
    pose = pose.reset_index().rename(columns={"index": "landmark_index"}).assign(type="pose")
    left_hand = left_hand.reset_index().rename(columns={"index": "landmark_index"}).assign(type="left_hand")
    right_hand = right_hand.reset_index().rename(columns={"index": "landmark_index"}).assign(type="right_hand")

    # BEGIN detect hand movement

    # print( left_hand )

    # Check if the hand has paused

    # print(f"hand max movement: {max_movement_left}, {max_movement_right}")
    if max_movement_right >= create_frame_landmark_df.PAUSE_THRESHOLD and max_movement_left >= create_frame_landmark_df.PAUSE_THRESHOLD:
        all_hands_paused = False

    if all_hands_paused:
        if create_frame_landmark_df.pause_start_time is None:
            create_frame_landmark_df.pause_start_time = time.time()
        elif time.time() - create_frame_landmark_df.pause_start_time >= create_frame_landmark_df.PAUSE_DURATION:
            hands_staddy_paused = True
    else:
        create_frame_landmark_df.pause_start_time = None
        hands_staddy_paused = False

    print(f"hands_paused: {hands_paused}")

    # END detect hand movement

    landmarks = pd.concat([face, pose, left_hand, right_hand]).reset_index(drop=True)
    landmarks = xyz_skel.merge(landmarks, on=["type", "landmark_index"], how="left").assign(frame=frame)

    return landmarks, hands_staddy_paused


def get_display_message_from_api(recognised_words):
    GOOGLE_API_KEY=os.getenv("GOOGLE_API_KEY")
    genai.configure(api_key=GOOGLE_API_KEY)

    model = genai.GenerativeModel('gemini-pro')

    prompt = f"""
            Objective:
            You have developed an isolated American Sign Language (ASL) word recognition model. At the end of each run, the model stores the recognized words in a list. However, the words may not necessarily be in the correct order. Your objective is to utilize these recognized words to construct a coherent and meaningful English sentence. The resulting sentence should be as simple as possible while still accurately conveying the intended meaning.

            Instructions:

            - Input: You will be provided with a Python list containing the recognized ASL words from your model. The contents of this list may vary depending on the output of your model.
            - Processing: Rearrange the words in the list to form a grammatically correct and logically valid English sentence. Take into consideration the context and logical flow of the sentence. Always ignore the word "TV".
            - Output: Generate a concise English sentence that accurately conveys the meaning of the recognized ASL words.

            Considerations:

            - Simplicity: Aim for simplicity in your sentence structure and vocabulary.
            - Clarity: Ensure that the sentence is clear and understandable.
            - Relevance: The sentence should reflect the meaning conveyed by the ASL words.
            - Grammar: Maintain proper grammar and syntax in the sentence.

            Example:

            Input: recognized_words = cat mat
            output: cat on the mat

            Here is the actual input for which you have to produce the relevant output: recognised_words = {' '.join(recognised_words)}
            """
    
    response = model.generate_content(prompt)
    
    return response.text

def load_relevant_data_subset(pq_path):
    data_columns = ['x', 'y', 'z']
    data = pd.read_parquet(pq_path, columns=data_columns)
    ROWS_PER_FRAME = 543
    n_frames = int(len(data) / ROWS_PER_FRAME)
    data = data.values.reshape(n_frames, ROWS_PER_FRAME, len(data_columns))
    return data.astype(np.float32)

def do_capture_loop(xyz, pred_fn):
    hands_paused = True
    all_landmarks = []
    cap = cv2.VideoCapture(0)
    ret, frame = cap.read()  # Check if the camera is working and get a frame to read dimensions
    if not ret:
        print("Failed to grab frame")
        cap.release()
        return
    
    frame_height, frame_width = frame.shape[:2]
    scale_factor = 1.0  # Scale the image to fill the window more
    scaled_height = int(frame_height * scale_factor)
    scaled_width = int(frame_width * scale_factor)
    display_width = scaled_width + 1200  # Extra width for text
    display_height = scaled_height  # Match the height of the camera feed

    font = cv2.FONT_HERSHEY_SIMPLEX
    font_scale = 2.5  # Larger font size
    text_thickness = 3
    
    start_time = time.time()
    font = cv2.FONT_HERSHEY_SIMPLEX
    font_scale = 2.0
    font_scale_d = 1.1 # Increased font scale for larger text
    last_prediction_time = 0
    escape_pressed = False
    display_message = "Press Escape to toggle message display"
    unique_signs = []
    sign_name = ""

    with mp_holistic.Holistic(min_detection_confidence=0.5, min_tracking_confidence=0.5) as holistic:
        while cap.isOpened():
            current_time = time.time()
            elapsed_time = int(current_time - start_time)

            success, image = cap.read()
            if not success:
                print("Ignoring empty camera frame.")
                continue

            # Scaling up the camera feed
            image = cv2.resize(image, (scaled_width, scaled_height), interpolation=cv2.INTER_LINEAR)
            image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

            results = holistic.process(image)

            landmarks, hands_paused = create_frame_landmark_df(results, elapsed_time, xyz, hands_paused)
            all_landmarks.append(landmarks)

            image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

            mp_drawing.draw_landmarks(
                image,
                results.face_landmarks,
                mp_holistic.FACEMESH_CONTOURS,
                landmark_drawing_spec=None,
                connection_drawing_spec=mp_drawing_styles.get_default_face_mesh_contours_style()
            )
            mp_drawing.draw_landmarks(
                image,
                results.pose_landmarks,
                mp_holistic.POSE_CONNECTIONS,
                landmark_drawing_spec=mp_drawing_styles.get_default_pose_landmarks_style()
            )

            if False and current_time - last_prediction_time >= 3:
                if all_landmarks:
                    concatenated_landmarks = pd.concat(all_landmarks).reset_index(drop=True)
                    concatenated_landmarks.to_parquet("out.parquet")
                    xyz_np = load_relevant_data_subset("out.parquet")
                    p = pred_fn(inputs=xyz_np)
                    sign = p['outputs'].argmax()
                    sign_name = ORD2SIGN[sign]
                    if sign_name not in unique_signs:
                        unique_signs.append(sign_name)

                    last_prediction_time = current_time
                    all_landmarks = []  # Reset landmarks

            if sign_name == "" or sign_name == "TV":
                sign_name = "No Movement Detected"

            # UI Improvements
            display = np.zeros((display_height, display_width, 3), dtype=np.uint8)
            display[:scaled_height, :scaled_width] = image

            # Draw the text
            cv2.putText(display, f"Sign: {sign_name}", (scaled_width + 10, 100), font, font_scale, (0, 255, 0), text_thickness)
            cv2.putText(display, f"Time: {elapsed_time}s", (scaled_width + 10, 200), font, font_scale, (0, 0, 255), text_thickness)

            if escape_pressed:
                cv2.putText(display, display_message, (scaled_width + 10, 300), font, font_scale_d, (255, 255, 0), text_thickness)

            cv2.imshow("MediaPipe Holistic", display)


            key = cv2.waitKey(5)
            if key & 0xFF == 27:
                escape_pressed = not escape_pressed
                display_message = get_display_message_from_api(unique_signs) if escape_pressed else "Press Escape to toggle message display"
                if escape_pressed: 
                    unique_signs = []
            elif key & 0xFF == ord('q'):
                break

    cap.release()
    cv2.destroyAllWindows()

# Load data and start capture loop
pq_file = "10042041.parquet"
xyz = pd.read_parquet(pq_file)
do_capture_loop(xyz, pred_fn)
