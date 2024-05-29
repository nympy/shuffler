module shuffler.Models

open CommandLine
open CommandLine.Text

type Options =
    {
        [<Option('i', "input", Required = true, HelpText = "Name of output file if not replacing the input file")>]
        inf: string;

        [<Option('o', "output", Required = false, HelpText = "Name of output file if not replacing the input file")>]
        outf: string;

        [<Option('v', "verbose", Required = false, Default = false, HelpText = "Turn on verbose logging.", Hidden = true)>]
        verbose: bool;

    }
    with
        [<Usage>]
        static member examples
            with get() = seq {
                    yield Example("Shuffle test.m3u and rewrite the content", { inf = "test.m3u"; outf = "";  verbose = false;})
                    yield Example("Shuffle test.m3u and write the output to foo.m3u", { inf = "test.m3u"; outf = "foo.m3u";  verbose = false;})
               }
