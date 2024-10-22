using System.Text.Json;

using MelonChart;
using MelonChart.Models;

using SpotifyPlaylist.ConsoleApp.Configs;
using SpotifyPlaylist.ConsoleApp.Helpers;
using SpotifyPlaylist.ConsoleApp.Options;

namespace SpotifyPlaylist.ConsoleApp.Services;

/// <summary>
/// This represents the service entity for Spotify playlist.
/// </summary>
/// <param name="settings"><see cref="SpotifySettings"/> instance.</param>
/// <param name="http"><see cref="HttpClient"/> instance.</param>
public class SpotifyPlaylistService(JsonSerializerOptions jso, IEnumerable<IChartHelper> helpers) : ISpotifyPlaylistService
{
    private readonly JsonSerializerOptions _jso = jso ?? throw new ArgumentNullException(nameof(jso));
    private readonly IEnumerable<IChartHelper> _helpers = helpers ?? throw new ArgumentNullException(nameof(helpers));

    /// <inheritdoc />
    public async Task RunAsync(string[] args)
    {
        var options = ArgumentOptions.Parse(args);
        if (options.Help)
        {
            this.DisplayHelp();
            return;
        }

        if (options.SourceType == SourceType.Undefined)
        {
            this.DisplayHelp();
            return;
        }

        try
        {
            var collection = default(ChartItemCollection);
            if (options.SourceType == SourceType.Spotify)
            {
                // Use source as Spotify playlist ID
                var helper = this._helpers.Single(p => p.Name == "Spotify");
                collection = await helper.BuildAsync(options).ConfigureAwait(false);
            }
            else
            {
                // Use source as Melon Chart JSON file
                var helper = this._helpers.Single(p => p.Name == "Melon");
                collection = await helper.BuildAsync(options).ConfigureAwait(false);
            }

            if (options.OutputAsJson)
            {
                Console.WriteLine(JsonSerializer.Serialize(collection, this._jso));
                return;
            }

            this.DisplayDetails(collection, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            this.DisplayHelp();
        }
    }

    private void DisplayDetails(ChartItemCollection collection, ArgumentOptions options)
    {
        if (options.SourceType == SourceType.Melon)
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
        }

        var items = collection.Items;

        if (options.SourceType == SourceType.Melon)
        {
            Console.WriteLine("Rank\tStatus\tTitle\tArtist\tAlbum");
            Console.WriteLine("----\t-----\t-----\t------\t-----");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Rank}\t{this.GetRankStatus(item)}\t{item.Title}\t{item.Artist}\t{item.Album}");
            }
        }
        else
        {
            Console.WriteLine("Rank\tTitle\tArtist\tAlbum\tValence\tDanceability");
            Console.WriteLine("----\t-----\t------\t-----\t-------\t------------");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title}\t{item.Artist}\t{item.Album}\t{item.Valence}\t{item.Danceability}");
            }
        }
    }

    private void DisplayHelp()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  -s, --source   The JSON file source that contains the Melon Chart data.");
        Console.WriteLine("  --json         Output in JSON format");
        Console.WriteLine("  -h, --help     Display help");
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
