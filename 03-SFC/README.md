# Task 03-SFC
Your task is to implement several **Space Filling Curves** (2D pixel orderings)
and find one with the best locality preserving condition.

Locality preserving is evaluated by predictive image compression.
The best ordering gives us the least entropy when plugged into the simple
First-Order Predictive Compression scheme.

# First-Order Predictive Compression
The simplest predictive compression used in contexts where no memory is required and
very fast computation is required. Another condition is that the data is in 1D (sequence) form,
so 2D images must be linearized by some "pixel ordering" method.

The method involves predicting the value of a given sample based on the value of the previous sample
("1-order"), and then encoding the difference between the predicted value and the actual value.
This technique can significantly reduce the amount of data to be stored or transmitted
when the input data is highly correlated (i.e., neighboring samples have similar values).

General steps in First-Order Predictive Compression:

### 1. Prediction
**Prediction Method**: Predict the next value in the data stream based on the previous value(s).
In the case of 1-order prediction, only the immediately preceding value is used for prediction.

### 2. Calculate the difference
**Difference**: Calculate the difference between the actual value and the predicted value.
This difference is often referred to as the "residual" or "error."

### 3. Encoding
**Quantization (optional)**: Sometimes, the residual is quantized to further compress
the data, especially in lossy compression. We don't need quantization, because there are only
number of possible residuals: `-255` to `255`.

**Entropy Encoding**: Encode the residuals using an entropy encoding method such as Huffman coding,
Run-Length Encoding, or Arithmetic coding to efficiently compress the data. Our task is to find
good pixel orders, so we will measure the theoretical **entropy**.

# Task
You already have a C# "command line" project that loads the input raster image and scans its
pixels.

The pilot implementation uses only normal line-by-line scanning ("scanline order").
The pixels are then sent to a simulated predictive codec, where a first-order prediction is
implemented, and then the total entropy of the entire sequence is measured.
The goal is to find a pixel order that yields the lowest entropy for the widest class
of input data.

The pilot implementation uses only the **luminance channel (grayscale)** and converts an
input image to grayscale if it is colored. You should include options to analyze the
R, G, or B color channels separately.

The **`interface PixelOrder`** is used to traverse the pixels of a 2D raster image. A single class,
`class Scanline`, implementing this interface is available in the pilot. Your task will be
to program as many (reasonable) alternatives as possible and test them on real data.
You can preferably use an AI assistant to find these alternatives and even have the
C# code generated.

Your main program should be able to easily switch between all the alternatives you have
implemented. Use line arguments and describe these alternatives well in the documentation.

However, you must also use the uniform format of specifying the method with a single
numeric parameter `-m <number>`, where the output is just a number expressing the
entropy in bits. Leave the pilot pixel order as `-m 0` and number your methods from one.
If your program gets a number greater than what is implemented, just return a line with
no number (like "Invalid method.").

The command line argument defining the input file should remain `-i <filename>`. So the
example of the default scanline order used on input file `masa512g.png` is:
```bash
> 03-sfc.exe -i masa512g.png -m 0
354219
```

If you do an **efficiency analysis** on a set of input images (you'll find some at the bottom
of this page), we'll be happy and you'll get extra points.
However, you must present the results in a clear form (spreadsheet or graphs) and preferably
include your notes and observations.

# Launch date
**Monday 23 October 2023**
(Don't work on the solution before this date)

# Deadline
See the shared [point table](https://docs.google.com/spreadsheets/d/1QLukOcSRPa5exOYW1eUfQWY2WoMjo1menbjQIU7Gvs4/edit?usp=sharing).

# Credit points
It has not yet been determined.

## Use of AI assistant
Using an AI assistant is recommended! But you have to be critical and
test all its suggestions thoroughly. Especially test singular cases (one
pixel image, 1xN image, etc.).

## Input Images

![Masa512g](masa512g.png)  
...