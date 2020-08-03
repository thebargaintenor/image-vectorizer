module Image

open System.Drawing
open System.Drawing.Drawing2D

type TranslateTransform = { X: int; Y: int }

let preprocess (bounds: Rectangle) (image: Bitmap): Bitmap =
    let width, height, transform =
        let widthF, heightF =
            (double image.Width), (double image.Height)

        if image.Width > image.Height then
            let w =
                int ((double bounds.Height) * widthF / heightF)

            w, bounds.Height, { X = (bounds.Width - w) / 2; Y = 0 }
        else
            let h =
                int ((double bounds.Width) * heightF / widthF)

            bounds.Width, h, { X = 0; Y = (bounds.Height - h) / 2 }

    // define resize target as sampling bounds
    let resized = new Bitmap(bounds.Width, bounds.Height)
    use g = Graphics.FromImage(resized)
    g.InterpolationMode <- InterpolationMode.Bicubic
    g.DrawImage(image, transform.X, transform.Y, width, height)
    resized

let quantizeByte (levels: int) (value: byte): int =
    if levels = 0 then int value else (int value) / (255 / levels)

let grayscale (c: Color): byte =
    let r, g, b =
        (float c.R) * 0.3, (float c.G) * 0.59, (float c.B) * 0.11

    byte (r + g + b)

let sample (levels: int) (image: Bitmap): int array =
    [| for y in 0 .. image.Height - 1 do
        for x in 0 .. image.Width - 1 do
            (quantizeByte levels (image.GetPixel(x, y) |> grayscale)) |]

let embed (width: int) (height: int) (levels: int) (image: Bitmap): int array =
    let boundingBox = Rectangle(0, 0, width, height)
    preprocess boundingBox image |> sample levels
