open System

let formatEmbedding (delimiter: string) (vec: int array) =
    String.Join(delimiter, Array.map string vec)

[<EntryPoint>]
let main argv =
    let path = argv.[0]
    use image = Embedding.loadImage path
    printfn "%s" ((Embedding.imgToVec 14 14 5 image) |> formatEmbedding " ")
    0 // return an integer exit code
