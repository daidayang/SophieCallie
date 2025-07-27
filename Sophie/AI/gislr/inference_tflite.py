import tflite_runtime.interpreter as tflite
import numpy as np
import pandas as pd
from IPython.display import display

interpreter = tflite.Interpreter("model.tflite")
found_signatures = list(interpreter.get_signature_list().keys())
prediction_fn = interpreter.get_signature_runner("serving_default")

# Read Training Data
train = pd.read_csv('train.csv')

N_SAMPLES = len(train)
print(f'N_SAMPLES: {N_SAMPLES}')


# Get complete file path to file
def get_file_path(path):
    return f'{path}'

# print(train['path'].values[0])  # debug

train['file_path'] = train['path'].apply(get_file_path)

# Add ordinally Encoded Sign (assign number to each sign name)
train['sign_ord'] = train['sign'].astype('category').cat.codes

# Dictionaries to translate sign <-> ordinal encoded sign
SIGN2ORD = train[['sign', 'sign_ord']].set_index('sign').squeeze().to_dict()
ORD2SIGN = train[['sign_ord', 'sign']].set_index('sign_ord').squeeze().to_dict()


# display(train.head(5))
# display(train.info())

# Source: https://www.kaggle.com/competitions/asl-signs/overview/evaluation
ROWS_PER_FRAME = 543  # number of landmarks per frame

def load_relevant_data_subset(pq_path):
    data_columns = ['x', 'y', 'z']
    data = pd.read_parquet(pq_path, columns=data_columns)
    n_frames = int(len(data) / ROWS_PER_FRAME)
    print( f"data len: {len(data)}, frames: {n_frames}")
    data = data.values.reshape(n_frames, ROWS_PER_FRAME, len(data_columns))
    return data.astype(np.float32)

# print( train['file_path'].values[2] )

demo_raw_data = load_relevant_data_subset(train['file_path'].values[2])
print(f'demo_raw_data shape: {demo_raw_data.shape}, dtype: {demo_raw_data.dtype}')
print(demo_raw_data[1])
# demo_output = tflite_keras_model(demo_raw_data)["outputs"]
# print(f'demo_output shape: {demo_output.shape}, dtype: {demo_output.dtype}')
# demo_prediction = demo_output.numpy().argmax()
# print(f'demo_prediction: {demo_prediction}, correct: {train.iloc[0]["sign_ord"]}')

output = prediction_fn(inputs=demo_raw_data)
sign = output['outputs'].argmax()

print("PRED : ", ORD2SIGN.get(sign), f'[{sign}]')
print("TRUE : ", train.sign.values[2], f'[{train.sign_ord.values[2]}]')

# print("----------------")
# print(demo_raw_data)

# print("----------------")
# print(output)
