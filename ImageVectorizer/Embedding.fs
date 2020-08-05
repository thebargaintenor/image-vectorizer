module Embedding

open System

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing

type TranslateTransform = { X: int; Y: int }

let private resizeToFill (bounds: Rectangle) (image: Image<Rgb24>): Image<Rgb24> =
    let transformation (context: IImageProcessingContext) =
        let resizeOptions =
            ResizeOptions
                (Mode = ResizeMode.Crop, Size = Size(bounds.Width, bounds.Height), Sampler = KnownResamplers.Bicubic)

        context.Resize(resizeOptions) |> ignore

    image.Clone(transformation)

let private quantizeByte (levels: int) (value: byte): int =
    if levels = 0 then
        255 - int value
    else
        int
            ((float levels)
             * (1.0 - (float value + 1.0) / 256.0))

let private grayscale (c: Rgb24): byte =
    let r, g, b =
        (float c.R) * 0.3, (float c.G) * 0.59, (float c.B) * 0.11

    byte (r + g + b)

let private sample (levels: int) (image: Image<Rgb24>): int array =
    [| for y in 0 .. image.Height - 1 do
        for x in 0 .. image.Width - 1 do
            (quantizeByte levels (image.Item(x, y) |> grayscale)) |]

let imgToVec (width: int) (height: int) (levels: int) (image: Image<Rgb24>): int array =
    let boundingBox = Rectangle(0, 0, width, height)
    use processed = resizeToFill boundingBox image
    processed |> sample levels

let loadImage (path: string): Image<Rgb24> = Image.Load<Rgb24>(path)

let format (delimiter: string) (vec: int array) =
    String.Join(delimiter, Array.map string vec)
