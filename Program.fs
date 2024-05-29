// Read a m3u file, shuffle the contents, and write it out again
open System
open System.IO
open CommandLine
open shuffler.Models

let ReadData f =
    let lines = File.ReadAllLines f
    Seq.toList lines

let outputFilename options =
    if options.outf = "" || options.inf = options.outf then
        options.inf
    else
        options.outf
        
let Run (options : Options) =
    
    let fileContents = ReadData options.inf
    let header = fileContents |> List.head
    
    let trackData =
        fileContents
        |> List.skip 1
        |> List.chunkBySize 2
        |> List.filter (fun l -> l.Length = 2 && l[0].StartsWith("#EXTINF:"))
        
    // randomise data
    let random = Random()
    let randomisedData =
        trackData
        |> List.sortBy (fun _ -> random.Next())
        
    // if overwriting, rename existing file to .bak
    if options.outf = "" || options.outf = options.inf then
        let backupFile = Path.ChangeExtension(options.inf, ".bak")
        File.Move(options.inf, backupFile, true)  
    
    // write header to output file
    let streamWriter = new StreamWriter(outputFilename options, false)
    streamWriter.AutoFlush <- true
    streamWriter.WriteLine header
    // write randomised list to output file
    randomisedData
    |> List.iter (fun lst -> lst |> List.iter streamWriter.WriteLine)
    // write blank line to output file
    streamWriter.WriteLine ""
    streamWriter.Close |> ignore
    
[<EntryPoint>]
let main argv =

    let result = Parser.Default.ParseArguments<Options>(argv)

    match result with
    | :? Parsed<Options> as parsed -> Run parsed.Value
    | _                            -> Environment.Exit(16)

    0 // return an integer exit code