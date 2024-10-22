using MelonChart.Models;

using SpotifyAPI.Web;

using SpotifyPlaylist.ConsoleApp.Options;

namespace SpotifyPlaylist.ConsoleApp.Helpers;

/// <summary>
/// This represents the helper entity to build Spotify chart items.
/// </summary>
public class SpotifyChartHelper(HttpClient http) : ChartHelper(http)
{
    /// <inheritdoc />
    public override string Name { get; set; } = "Spotify";

    /// <inheritdoc />
    public override async Task<ChartItemCollection> BuildAsync(ArgumentOptions options)
    {
        var collection = await base.BuildAsync(options).ConfigureAwait(false);

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

    internal async Task<TrackAudioFeatures> GetTrackAudioFeaturesAsync(string trackId)
    {
        var feature = await this.Spotify!.Tracks.GetAudioFeatures(trackId!).ConfigureAwait(false);

        return feature;
    }
}
