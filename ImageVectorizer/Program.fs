open System

open Argu

type CliArguments =
    | [<AltCommandLine("-s"); Mandatory>] Size of width: int * height: int
    | [<AltCommandLine("-l")>] Levels of levels: int
    | [<AltCommandLine("-d")>] Delimiter of delimiter: string
    | [<MainCommand; ExactlyOnce; Last>] Images of image: string list

    interface IArgParserTemplate with
        member this.Usage = 
            match this with
            | Size -> "Width and height of vectorized image"
            | Levels -> "Number of output levels for quantizing grayscale values (0 to n-1)"
            | Delimiter -> "Delimiter for output vector"
            | Images -> "Paths of images to embed"

module Constraints =
    let parseSize (width: int, height: int) =
        if width > 0 && height > 0 then
            (width, height)
        else
            failwith "Width and height must be greater than 0"

    let parseLevels levels: int =
        if levels < 0 then
            failwith "Level count cannot be negative"
        else
            levels

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<CliArguments>(programName="ImageVectorizer.exe", errorHandler=errorHandler)
    let cliArgs = parser.ParseCommandLine argv

    let paths = cliArgs.GetResult Images
    let width, height = cliArgs.PostProcessResult (<@ Size @>, Constraints.parseSize)
    let levels = 
        if cliArgs.Contains Levels then
            cliArgs.PostProcessResult (<@ Levels @>, Constraints.parseLevels)
        else
            0
    let delimiter = 
        if cliArgs.Contains Delimiter then
            cliArgs.GetResult Delimiter
        else
            " "

    let embed = Embedding.imgToVec width height levels
    let sprintvec = Embedding.format delimiter

    for path in paths do
        use image = Embedding.loadImage path
        embed image
        |> sprintvec
        |> printfn "%s"
    0
