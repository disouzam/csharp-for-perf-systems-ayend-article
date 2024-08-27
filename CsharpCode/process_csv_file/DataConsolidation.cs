using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace process_csv_file;

internal static class DataConsolidation
{
    /// <summary>
    /// Listing 2: Computing total and quantity per user using Linq
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>
    /// Code extracted from https://www.codemag.com/article/2403091
    /// </remarks>
    public static ValueTask<Dictionary<long, UserSales>> Linq(Stream input)
    {
        return (from l in ZippedFileProcessing.GzipReadlAllLinesAsync(input)
          .Skip(1) // skip header line
                let flds = l.Split(',')
                let item = new
                {
                    UserId = long.Parse(flds[0]),
                    Quantity = int.Parse(flds[2]),
                    Price = decimal.Parse(flds[3])
                }
                group item by item.UserId into g
                select new
                {
                    UserId = g.Key,
                    Quantity = g.SumAsync(x => x.Quantity),
                    Total = g.SumAsync(x => x.Price)
                }).ToDictionaryAsync(x => x.UserId, x => new UserSales
                {
                    Quantity = x.Quantity.Result,
                    Total = x.Total.Result
                });
    }

    /// <summary>
    /// Listing 3: Manually aggregating over the records
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>
    /// Code extracted from https://www.codemag.com/article/2403091
    /// </remarks>
    public static async Task<Dictionary<long, UserSales>> StreamReaderAndDictionary(Stream input)
    {
        var sales = new Dictionary<long, UserSales>();
        await foreach (var line in ZippedFileProcessing.GzipReadlAllLinesAsync(input).Skip(1))
        {
            var fields = line.Split(',');
            var uid = long.Parse(fields[0]);
            int quantity = int.Parse(fields[2]);
            decimal price = decimal.Parse(fields[3]);

            if (!sales.TryGetValue(uid, out var stats))
                sales[uid] = stats = new UserSales();

            stats.Total += price * quantity;
            stats.Quantity += quantity;
        }
        return sales;
    }
}
