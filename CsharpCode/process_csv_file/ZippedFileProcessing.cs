using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace process_csv_file;

internal static class ZippedFileProcessing
{
    /// <summary>
    /// Code extracted from Listing 1 on https://www.codemag.com/article/2403091
    /// </summary>
    /// <param name="input"></param>
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
