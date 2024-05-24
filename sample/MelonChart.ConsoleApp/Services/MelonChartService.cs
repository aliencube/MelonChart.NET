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
        if (string.IsNullOrWhiteSpace(collection.DateLastUpdated) == false)
        {
            Console.WriteLine($"Date: {collection.DateLastUpdated}");
        }
        if (string.IsNullOrWhiteSpace(collection.TimeLastUpdated) == false)
        {
            Console.WriteLine($"Time: {collection.TimeLastUpdated}");
        }
        if (string.IsNullOrWhiteSpace(collection.PeriodFrom) == false && string.IsNullOrWhiteSpace(collection.PeriodTo) == false)
        {
            Console.WriteLine($"Week: {collection.PeriodFrom} - {collection.PeriodTo}");
        }
        if (string.IsNullOrWhiteSpace(collection.Year) == false && string.IsNullOrWhiteSpace(collection.Month) == false)
        {
            Console.WriteLine($"Month: {collection.Year}-{collection.Month}");
        }
        Console.WriteLine();

        var items = collection.Items;

        Console.WriteLine("Rank\tTitle\tArtist\tAlbum");
        Console.WriteLine("----\t-----\t------\t-----");
        foreach (var item in items)
        {
            Console.WriteLine($"{item.Rank}\t{item.Title}\t{item.Artist}\t{item.Album}");
        }
    }

    private void DisplayHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  -c, -t, --chart, --type, --chart-type <chart-type>    Chart type - 'Top100', 'Hot100', 'Daily100', 'Weekly100' or 'Monthly100'.");
        Console.WriteLine("  -h, --help                                            Display help");
    }
}
