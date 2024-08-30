namespace playing_with_perf_profiler;

internal static class FibonacciSeries
{
    public static int GetFiboNumber(int order)
    {
        if (order == 0)
        {
            return 0;
        }

        if (order == 1)
        {
            return 1;
        }

        return GetFiboNumber(order - 1) + GetFiboNumber(order - 2);
    }
}
