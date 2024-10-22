using System.Text.Json;

using MelonChart.Models;

using SpotifyAPI.Web;

using SpotifyPlaylist.ConsoleApp.Configs;
using SpotifyPlaylist.ConsoleApp.Options;

namespace SpotifyPlaylist.ConsoleApp.Helpers;

/// <summary>
/// This represents the helper entity to build Melon chart items.
/// /// </summary>
public class MelonChartHelper(SpotifySettings settings, JsonSerializerOptions jso, HttpClient http) : ChartHelper(http)
{
    private readonly SpotifySettings _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    private readonly JsonSerializerOptions _jso = jso ?? throw new ArgumentNullException(nameof(jso));

    /// <inheritdoc />
    public override string Name { get; set; } = "Melon";

    /// <inheritdoc />
    public override async Task<ChartItemCollection> BuildAsync(ArgumentOptions options)
    {
        await base.BuildAsync(options).ConfigureAwait(false);

        var profile = await this.GetMyProfileAsync().ConfigureAwait(false);
        var playlist = await this.CreatePlaylistAsync(profile.Id).ConfigureAwait(false);

        var collection = await this.GetMelonChartCollectionAsync(options).ConfigureAwait(false);
        var tracks = await this.GetTrackItemCollectionAsync("../../data/tracks.json").ConfigureAwait(false);
        var trackUris = new List<string>();

        foreach (var item in collection.Items!)
        {
            var track = await this.SearchTracksAsync(item.SongId!, tracks).ConfigureAwait(false);
            if (track == null)
            {
                var missing = new TrackItem
                {
                    SongId = item.SongId,
                    Title = item.Title,
                    Artist = item.Artist,
                    Album = item.Album,
                };
                collection.MissingTracks.Add(missing);

                continue;
            }

            trackUris.Add(track.TrackUri!);

            item.TrackId = track.TrackId;
            item.TrackUri = track.TrackUri;

            item.Danceability = track.Danceability;
            item.Valence = track.Valence;
        }

        await this.AddTracksToPlaylistAsync(playlist.Id!, trackUris).ConfigureAwait(false);

        return collection;
    }

    internal async Task<PrivateUser> GetMyProfileAsync()
    {
        var profile = await this.Spotify!.UserProfile.Current().ConfigureAwait(false);

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

        var playlists = await this.Spotify!.Playlists.CurrentUsers().ConfigureAwait(false);
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
        playlist = await this.Spotify!.Playlists.Create(userId, request).ConfigureAwait(false);

        await this.UpdateCoverImageAsync(playlist.Id!).ConfigureAwait(false);

        return playlist;
    }

    internal async Task UpdatePlaylistDetailsAsync(string playlistId, string description)
    {
        var request = new PlaylistChangeDetailsRequest { Description = description };
        await this.Spotify!.Playlists.ChangeDetails(playlistId, request).ConfigureAwait(false);
        await this.UpdateCoverImageAsync(playlistId).ConfigureAwait(false);
    }

    internal async Task UpdateCoverImageAsync(string playlistId)
    {
        var images = await this.Spotify!.Playlists.GetCovers(playlistId).ConfigureAwait(false);
        if (images.Count != 1)
        {
            var bytes = await File.ReadAllBytesAsync(Path.Combine(ProjectPathInfo.ProjectPath, "../../assets/MelonChart.NET.png")).ConfigureAwait(false);
            var cover = Convert.ToBase64String(bytes);
            await this.Spotify!.Playlists.UploadCover(playlistId, cover).ConfigureAwait(false);
        }
    }

    internal async Task<List<PlaylistTrack<IPlayableItem>>> GetTrackItemsAsync(string playlistId)
    {
        var request = new PlaylistGetItemsRequest(PlaylistGetItemsRequest.AdditionalTypes.Track);
        var tracks = await this.Spotify!.Playlists.GetItems(playlistId, request).ConfigureAwait(false);
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
        await this.Spotify!.Playlists.RemoveItems(playlistId, request).ConfigureAwait(false);
    }

    internal async Task<ChartItemCollection> GetMelonChartCollectionAsync(ArgumentOptions options)
    {
        var source = Path.IsPathRooted(options.Source!) ? options.Source : Path.Combine(ProjectPathInfo.ProjectPath, options.Source!);
        var data = await File.ReadAllTextAsync(source).ConfigureAwait(false);
        var collection = JsonSerializer.Deserialize<ChartItemCollection>(data, this._jso);

        return collection!;
    }

    internal async Task<TrackItemCollection> GetTrackItemCollectionAsync(string filename)
    {
        var source = Path.Combine(ProjectPathInfo.ProjectPath, filename);
        var data = await File.ReadAllTextAsync(source).ConfigureAwait(false);
        var items = JsonSerializer.Deserialize<List<TrackItem>>(data, this._jso);
        var collection = new TrackItemCollection { Items = items! };

        return collection!;
    }

    internal async Task<TrackItem?> SearchTracksAsync(string melonSongId, TrackItemCollection collection)
    {
        var track = collection!.Items.SingleOrDefault(p => p.SongId == melonSongId);

        return await Task.FromResult(track).ConfigureAwait(false);
    }

    internal async Task<SnapshotResponse> AddTracksToPlaylistAsync(string playlistId, List<string> trackUris)
    {
        var request = new PlaylistAddItemsRequest(trackUris);
        var snapshot = await this.Spotify!.Playlists.AddItems(playlistId, request).ConfigureAwait(false);

        return snapshot;
    }
}
