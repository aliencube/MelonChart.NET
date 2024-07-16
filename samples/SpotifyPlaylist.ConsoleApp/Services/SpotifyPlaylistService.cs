using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

using MelonChart;
using MelonChart.Models;

using SpotifyAPI.Web;

using SpotifyPlaylist.ConsoleApp.Configs;
using SpotifyPlaylist.ConsoleApp.Options;

namespace SpotifyPlaylist.ConsoleApp.Services;

/// <summary>
/// This represents the service entity for Spotify playlist.
/// </summary>
/// <param name="settings"><see cref="SpotifySettings"/> instance.</param>
/// <param name="http"><see cref="HttpClient"/> instance.</param>
public class SpotifyPlaylistService(SpotifySettings settings, HttpClient http) : ISpotifyPlaylistService
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

    private readonly SpotifySettings _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    private readonly HttpClient _http = http ?? throw new ArgumentNullException(nameof(http));

    private ISpotifyClient? _spotify;

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
            await this.EnsureSpotifyClientAsync().ConfigureAwait(false);

            var collection = default(ChartItemCollection);
            if (options.SourceType == SourceType.Spotify)
            {
                // Use source as Spotify playlist ID
                collection = await this.BuildFromSpotifyAsync(options).ConfigureAwait(false);
            }
            else
            {
                // Use source as Melon Chart JSON file
                collection = await this.BuildFromMelonChartAsync(options).ConfigureAwait(false);
            }

            if (options.OutputAsJson)
            {
                Console.WriteLine(JsonSerializer.Serialize(collection, jso));
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

    internal async Task<ChartItemCollection> BuildFromSpotifyAsync(ArgumentOptions options)
    {
        var collection = new ChartItemCollection();

        var playlistId = options.Source!;
        var trackItems = await this.GetTrackItemsAsync(playlistId).ConfigureAwait(false);
        foreach (var trackItem in trackItems)
        {
            var track = (FullTrack)trackItem.Track;
            var feature = await this.GetTrackAudioFeaturesAsync(track.Id!).ConfigureAwait(false);

            var item = new ChartItem
            {
                Title = track.Name,
                Artist = string.Join(", ", track.Artists.Select(p => p.Name)),
                Album = track.Album.Name,
                Image = track.Album.Images.FirstOrDefault()?.Url,
                Rank = trackItems.IndexOf(trackItem) + 1,
                TrackId = track.Id,
                TrackUri = track.Uri,
                Danceability = feature?.Danceability,
                Valence = feature?.Valence,
            };

            collection.Items.Add(item);

            if (trackItems.IndexOf(trackItem) > 0 && trackItems.IndexOf(trackItem) % 25 == 0)
            {
                Thread.Sleep(15000);
            }
        }

        return collection;
    }

    internal async Task<ChartItemCollection> BuildFromMelonChartAsync(ArgumentOptions options)
    {
        var profile = await this.GetMyProfileAsync().ConfigureAwait(false);
        var playlist = await this.CreatePlaylistAsync(profile.Id).ConfigureAwait(false);

        var source = Path.IsPathRooted(options.Source!) ? options.Source : Path.Combine(ProjectPathInfo.ProjectPath, options.Source!);
        var data = await File.ReadAllTextAsync(source).ConfigureAwait(false);
        var collection = JsonSerializer.Deserialize<ChartItemCollection>(data, jso);
        var trackIds = new List<string>();
        var trackUris = new List<string>();

        foreach (var item in collection?.Items!)
        {
            var track = await this.SearchTracksAsync(item.Title!, item.Artist!, item.Album!, options.Query!).ConfigureAwait(false);
            if (track == null)
            {
                continue;
            }

            trackIds.Add(track?.Id!);
            trackUris.Add(track?.Uri!);

            item.TrackId = track?.Id!;
            item.TrackUri = track?.Uri!;

            var feature = await this.GetTrackAudioFeaturesAsync(track?.Id!).ConfigureAwait(false);
            item.Danceability = feature?.Danceability;
            item.Valence = feature?.Valence;

            Thread.Sleep(10000);
        }

        await this.AddTracksToPlaylistAsync(playlist.Id!, trackUris).ConfigureAwait(false);

        return collection;
    }

    internal async Task EnsureSpotifyClientAsync()
    {
        if (this._spotify is not null)
        {
            return;
        }

        await this.SetSpotifyClientAsync().ConfigureAwait(false);
    }

    internal async Task<PrivateUser> GetMyProfileAsync()
    {
        var profile = await this._spotify!.UserProfile.Current().ConfigureAwait(false);

        return profile;
    }

    internal async Task<FullPlaylist> CreatePlaylistAsync(string userId)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(this._settings.TimeZoneId!);
        var now = $"{DateTimeOffset.UtcNow
                                   .AddHours(tz.BaseUtcOffset.Hours)
                                   .ToString("yyyy-MM-dd HH:mm")} {(tz.BaseUtcOffset.Hours >= 0 ? "+" : "-")}{tz.BaseUtcOffset.ToString(@"hh\:mm")}";

        var name = this._settings.Playlist?.Name;
        var description = string.Format(this._settings.Playlist?.Description!, now);

        var playlists = await this._spotify!.Playlists.CurrentUsers().ConfigureAwait(false);
        var playlist = playlists.Items?.SingleOrDefault(p => p.Name!.Equals(name));
        if (playlist is not null)
        {
            await this.UpdatePlaylistDetailsAsync(playlist.Id!, description).ConfigureAwait(false);
            var uris = await this.GetTrackItemUrisAsync(playlist.Id!).ConfigureAwait(false);
            if (uris.Count > 0)
            {
                await this.RemoveTrackItemsFromPlaylistAsync(playlist.Id!, playlist.SnapshotId!, uris).ConfigureAwait(false);
            }

            return playlist;
        }

        var request = new PlaylistCreateRequest(name!) { Description = description!, Public = true, Collaborative = false };
        playlist = await this._spotify!.Playlists.Create(userId, request).ConfigureAwait(false);

        return playlist;
    }

    internal async Task UpdatePlaylistDetailsAsync(string playlistId, string description)
    {
        var request = new PlaylistChangeDetailsRequest { Description = description };
        await this._spotify!.Playlists.ChangeDetails(playlistId, request).ConfigureAwait(false);
    }

    internal async Task<List<PlaylistTrack<IPlayableItem>>> GetTrackItemsAsync(string playlistId)
    {
        var request = new PlaylistGetItemsRequest(PlaylistGetItemsRequest.AdditionalTypes.Track);
        var tracks = await this._spotify!.Playlists.GetItems(playlistId, request).ConfigureAwait(false);
        var items = tracks.Items;

        return items!;
    }

    internal async Task<List<string>> GetTrackItemUrisAsync(string playlistId)
    {
        var tracks = await this.GetTrackItemsAsync(playlistId).ConfigureAwait(false);
        var uris = tracks?.Select(p => ((FullTrack)p.Track).Uri).ToList();

        return uris!;
    }

    internal async Task RemoveTrackItemsFromPlaylistAsync(string playlistId, string snapshotId, List<string> trackUris)
    {
        var request = new PlaylistRemoveItemsRequest
        {
            SnapshotId = snapshotId,
            Tracks = trackUris.Select(p => new PlaylistRemoveItemsRequest.Item() { Uri = p }).ToList()
        };
        await this._spotify!.Playlists.RemoveItems(playlistId, request).ConfigureAwait(false);
    }

    internal async Task<FullTrack?> SearchTracksAsync(string title, string artist, string album, string query)
    {
        var q = $"track:{title} artist:{artist}";
        if (string.IsNullOrEmpty(query) == false)
        {
            q += $" {query}";
        }

        var request = new SearchRequest(SearchRequest.Types.Track, q)
        {
            Type = SearchRequest.Types.Track,
            Market = this._settings.Market,
            Limit = this._settings.MaxItems,
        };

        var response = await this._spotify!.Search.Item(request).ConfigureAwait(false);
        var track = response.Tracks.Items?.FirstOrDefault();

        // Search one more time with album, if track is not found
        if (track == null)
        {
            q = $"track:{title} artist:{artist} album:{album}";
            if (string.IsNullOrEmpty(query) == false)
            {
                q += $" {query}";
            }

            request = new SearchRequest(SearchRequest.Types.Track, q)
            {
                Type = SearchRequest.Types.Track,
                Market = this._settings.Market,
                Limit = this._settings.MaxItems,
            };

            response = await this._spotify!.Search.Item(request).ConfigureAwait(false);
            track = response.Tracks.Items?.FirstOrDefault();
        }

        return track;
    }

    internal async Task<TrackAudioFeatures> GetTrackAudioFeaturesAsync(string trackId)
    {
        var feature = await this._spotify!.Tracks.GetAudioFeatures(trackId!).ConfigureAwait(false);

        return feature;
    }

    internal async Task<SnapshotResponse> AddTracksToPlaylistAsync(string playlistId, List<string> trackUris)
    {
        var request = new PlaylistAddItemsRequest(trackUris);
        var snapshot = await this._spotify!.Playlists.AddItems(playlistId, request).ConfigureAwait(false);

        return snapshot;
    }

    private async Task SetSpotifyClientAsync()
    {
        var accessToken = await _http.GetStringAsync("spotify/access-token").ConfigureAwait(false);
        var spotify = new SpotifyClient(accessToken);

        this._spotify = spotify;
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
