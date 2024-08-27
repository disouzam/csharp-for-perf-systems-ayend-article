using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace process_csv_file;

internal static class LineProcessing
{
    /// <summary>
    /// Listing 5: Processing lines in the buffer
    /// </summary>
    /// <param name="pipeReader"></param>
    /// <param name="readResult"></param>
    /// <param name="salesData"></param>
    /// <param name="header"></param>
    /// <remarks>
    /// Code extracted from https://www.codemag.com/article/2403091
    /// </remarks>
    public static void ProcessLines(
        PipeReader pipeReader,
        ReadResult readResult,
        Dictionary<long, UserSalesStruct> salesData,
        ref bool header)
    {
        var sr = new SequenceReader<byte>(readResult.Buffer);
        while (true)
        {
            ReadOnlySequence<byte> line;
            if (sr.TryReadTo(out line, (byte)'\n') == false)
            {
                pipeReader.AdvanceTo(consumed: sr.Position,
                  examined: readResult.Buffer.End);
                break;
            }
            if (header == false)
            {
                header = true;
                continue;
            }
            ProcessSingleLine(salesData, line);
        }
    }

    /// <summary>
    /// Listing 6: Processing a single line
    /// </summary>
    /// <param name="salesData"></param>
    /// <param name="line"></param>
    /// <exception cref="InvalidDataException"></exception>
    /// <remarks>
    /// Code extracted from https://www.codemag.com/article/2403091
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ProcessSingleLine(
        Dictionary<long, UserSalesStruct> salesData,
        ReadOnlySequence<byte> line)
    {
        var lr = new SequenceReader<byte>(line);
        ReadOnlySpan<byte> span;
        var readAll = lr.TryReadTo(out span, (byte)',')
            & Utf8Parser.TryParse(span, out long userId, out _)
            & lr.TryAdvanceTo((byte)',')
            & lr.TryReadTo(out span, (byte)',')
            & Utf8Parser.TryParse(span, out int quantity, out _)
            & lr.TryReadTo(out span, (byte)',')
            & Utf8Parser.TryParse(span, out decimal price, out _);

        if (readAll == false) throw new InvalidDataException(
          "Couldn't parse expected fields on: " +
          Encoding.UTF8.GetString(line));

        ref var current = ref CollectionsMarshal
          .GetValueRefOrAddDefault(salesData, userId, out _);
        current.Total += price;
        current.Quantity += quantity;
    }
}
