namespace playing_with_perf_profiler;

internal static class FibonacciSeries
{
    public static long GetFiboNumber(int order, out string stringRepresentation)
    {
        if (order == 0)
        {
            stringRepresentation = "0";
            return 0;
        }

        if (order == 1)
        {
            stringRepresentation = "1";
            return 1;
        }

        stringRepresentation = "Not ready yet";
        var localString = string.Empty;
        var localString2 = string.Empty;
        return GetFiboNumber(order - 1, out localString) + GetFiboNumber(order - 2, out localString2);
    }
}
