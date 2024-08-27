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

}
