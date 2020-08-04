# Image Vectorizer

This utility converts a bitmap (any really) into grayscale, resizes to 14x14 (for now) and quantizes the gray levels for output as a vector of integers.  Command line flags for adjusting resize settings and how the output vector is formatted will arrive once the rest of the program works correctly.  Default settings right now match an old test harness and data I have from when I was in grad school.

Basic intent is to crop and preprocess an image for use as a test vector for a trained classifer for the MNIST data set.  I suppose you could use it for other things, but this is why it exists at all.

## Usage

```sh
dotnet run image.png > vector.txt
```

## Requirements
- .NET Core 3.1
- libgdiplus

