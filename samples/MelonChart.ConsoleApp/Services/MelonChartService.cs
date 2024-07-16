using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

using MelonChart.Abstractions;
using MelonChart.ConsoleApp.Options;
using MelonChart.Models;

namespace MelonChart.ConsoleApp.Services;

/// <summary>
/// This represents the service entity for Melon chart.
/// </summary>
/// <param name="charts">List of <see cref="IChart"/> instances.</param>
public class MelonChartService(IEnumerable<IChart> charts) : IMelonChartService
{
#pragma warning disable IDE1006 // Naming Styles

    private static readonly JsonSerializerOptions jso = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
    };

#pragma warning restore IDE1006 // Naming Styles

    private readonly IEnumerable<IChart> _charts = charts ?? throw new ArgumentNullException(nameof(charts));

    /// <inheritdoc />
    public async Task RunAsync(string[] args)
    {
        var options = ArgumentOptions.Parse(args);
        if (options.Help)
        {
            this.DisplayHelp();
            return;
        }

        try
        {
            var chart = this._charts.SingleOrDefault(p => p.ChartType.Equals(options.ChartType));
            if (chart is null)
            {
                throw new ArgumentException("Invalid chart type. It should be 'Top100', 'Hot100', 'Daily100', 'Weekly100' or 'Monthly100'.");
            }

            var collection = await chart.GetChartAsync().ConfigureAwait(false);
            if (options.OutputAsJson)
            {
                Console.WriteLine(JsonSerializer.Serialize(collection, jso));
                return;
            }

            this.DisplayDetails(collection);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            this.DisplayHelp();
        }
    }

    private void DisplayDetails(ChartItemCollection collection)
    {
        Console.WriteLine($"Chart Type: {collection.ChartType}");
        switch (collection.ChartType)
        {
            case ChartTypes.Top100:
            case ChartTypes.Hot100:
            default:
                Console.WriteLine($"Date/Time: {collection.DateLastUpdated} {collection.TimeLastUpdated}");
                break;

            case ChartTypes.Daily100:
                Console.WriteLine($"Date: {collection.DateLastUpdated}");
                break;

            case ChartTypes.Weekly100:
                Console.WriteLine($"Week: {collection.PeriodFrom} - {collection.PeriodTo}");
                break;

            case ChartTypes.Monthly100:
                Console.WriteLine($"Month: {collection.Year}-{collection.Month}");
                break;
        }
        Console.WriteLine();

        var items = collection.Items;

        Console.WriteLine("Rank\tStatus\tTitle\tArtist\tAlbum");
        Console.WriteLine("----\t-----\t-----\t------\t-----");
        foreach (var item in items)
        {
            Console.WriteLine($"{item.Rank}\t{this.GetRankStatus(item)}\t{item.Title}\t{item.Artist}\t{item.Album}");
        }
    }

    private void DisplayHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  -c, -t, --chart, --type, --chart-type <chart-type>    Chart type - 'Top100', 'Hot100', 'Daily100', 'Weekly100' or 'Monthly100'.");
        Console.WriteLine("  --json                                                Output in JSON format");
        Console.WriteLine("  -h, --help                                            Display help");
    }

    private string GetRankStatus(ChartItem item)
    {
        return item.RankStatus switch
        {
            RankStatus.None => "--",
            RankStatus.Up => $"+{item.RankStatusValue}",
            RankStatus.Down => $"-{item.RankStatusValue}",
            RankStatus.New => "new",
            _ => "Unknown",
        };
    }
}
