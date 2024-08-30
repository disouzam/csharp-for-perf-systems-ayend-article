using System;

namespace playing_with_perf_profiler;

public static class Program
{
    public static void Main(string[] args)
    {
        var message = "Calculation of Fibonnacci number!";
        Console.WriteLine(message);

        //Console.WriteLine("Type the position to which you want a Fibonacci number...");
        //var orderAsString = Console.ReadLine();

        //int order;
        //var parseResult = int.TryParse(orderAsString, out order);

        //if (!parseResult)
        //{
        //    return;
        //}
        var localString = string.Empty;
        Console.WriteLine(FibonacciSeries.GetFiboNumber(50, out localString));
    }
}
