using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace process_csv_file;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // Monitoring app performance
        // Taken from https://gist.github.com/ayende/b33c29289ddd44c42f298d0a98e91bda
        AppDomain.MonitoringIsEnabled = true;
        var sp = Stopwatch.StartNew();

        Console.WriteLine("Processing a very large CSV file...");
        Console.WriteLine("Reference: Ayende's article on https://www.codemag.com/article/2403091");

        using var fs = new FileStream(path: "E:\\GitHubRepos\\csharp-for-perf-systems-ayende-article\\data.csv.gz", mode: FileMode.Open);

        var userSalesConsolidation = DataConsolidation.StreamReaderAndDictionary(fs);
        var result = userSalesConsolidation.Result;

        //foreach (var individualResult in result)
        //{
        //    var userId = individualResult.Key;
        //    var salesResults = individualResult.Value;
        //    Console.WriteLine($"UserId: {userId} - Quantity: {salesResults.Quantity} - Total: {salesResults.Total}");
        //}

        Console.WriteLine("The end");

        // Taken from https://gist.github.com/ayende/b33c29289ddd44c42f298d0a98e91bda
        Console.WriteLine($"Took: {sp.ElapsedMilliseconds:#,#} ms and allocated " +
                        $"{AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize / 1024:#,#} kb " +
                        $"with peak working set of {Process.GetCurrentProcess().PeakWorkingSet64 / 1024:#,#} kb");
    }
}
