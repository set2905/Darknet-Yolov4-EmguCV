# Darknet-Yolov4-EmguCV
This project reads Darknet models supported by the OpenCV library and displays inference result.

Works on both videos on images, to detect objects from images just uncomment the code block in the "Main" function and comment the video block.

# Models
You can download and test pre-trained models from [Here](https://hackmd.io/NFj2NrmqTcefjc2l94KjpQ).
This project was mainly tested on Yolov4-Tiny model due to the lack of GPU. Bigger models might require bigger image resolution which in turn will take more processing time.

# Cuda support
This implementation supports CUDA (Cuda toolkit v11+ is required). For smaller repo size, EmguCV's cuda libraries weren't included with the repo. In order to properly run, [Download EmguCV with CUDA](https://sourceforge.net/projects/emgucv/files/emgucv/4.4.0/libemgucv-windesktop_x64-cuda-4.4.0.4099.zip.selfextract.exe/download) and extract it. After extracting, copy the "libs" folder included in the extracted folder and paste it inside References/EmguCV (replace the existing libs folder).
