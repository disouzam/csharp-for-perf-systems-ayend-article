using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace process_csv_file;

internal static class ZippedFileProcessing
{
    /// <summary>
    /// Listing 1: Decompressing and yielding the lines from the file
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>
    /// Code extracted from https://www.codemag.com/article/2403091
    /// </remarks>
    public static async IAsyncEnumerable<string> GzipReadlAllLinesAsync(Stream input)
    {
        using var gzipStream = new GZipStream(input, CompressionMode.Decompress);
        using var reader = new StreamReader(gzipStream);

        while (true)
        {
            string? line = await reader.ReadLineAsync();
            if (line == null)
                break;
            yield return line;
        }
    }

}
