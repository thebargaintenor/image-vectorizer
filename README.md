# Image Vectorizer

This utility converts a bitmap (any really) into grayscale, resizes to 14x14 (for now) and quantizes the gray levels for output as a vector of integers.  Command line flags for adjusting resize settings and how the output vector is formatted will arrive once the rest of the program works correctly.  Default settings right now match an old test harness and data I have from when I was in grad school.

Basic intent is to crop and preprocess an image for use as a test vector for a trained classifer for the MNIST data set.  I suppose you could use it for other things, but this is why it exists at all.

## Usage

```sh
USAGE: ImageVectorizer.exe [--help] --size <width> <height> [--levels <levels>] [--delimiter <delimiter>] <image>...

IMAGES:

    <image>...            Paths of images to embed

OPTIONS:

    --size, -s <width> <height>
                          Width and height of vectorized image
    --levels, -l <levels> Number of output levels for quantizing grayscale values (0 to n-1)
    --delimiter, -d <delimiter>
                          Delimiter for output vector
    --help                display this list of options.
```

## Requirements
- .NET Core 3.1
- libgdiplus

