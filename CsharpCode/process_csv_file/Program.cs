using System;
using System.IO;
using System.Threading.Tasks;

namespace process_csv_file;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Processing a very large CSV file...");
        Console.WriteLine("Reference: Ayende's article on https://www.codemag.com/article/2403091");

        using var fs = new FileStream(path: "E:\\GitHubRepos\\csharp-for-perf-systems-ayende-article\\data.csv.gz", mode: FileMode.Open);

        var userSalesConsolidation = DataConsolidation.Linq(fs);
        var result = userSalesConsolidation.Result;

        Console.WriteLine(result.Count);
        Console.WriteLine("The end");
    }
}
