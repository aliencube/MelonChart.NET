using MelonChart.Models;

using SpotifyAPI.Web;

using SpotifyPlaylist.ConsoleApp.Options;

namespace SpotifyPlaylist.ConsoleApp.Helpers;

/// <summary>
/// This provides interfaces to chart helper classes.
/// </summary>
public interface IChartHelper
{
    /// <summary>
    /// Gets or sets the helper name.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Build <see cref="ChartItemCollection"/> instance.
    /// </summary>
    /// <param name="options"><see cref="ArgumentOptions"/> instance.</param>
    /// <returns>Returns the <see cref="ChartItemCollection"/> instance.</returns>
    Task<ChartItemCollection> BuildAsync(ArgumentOptions options);
}

/// <summary>
/// This represents the helper entity to build chart items.
/// </summary>
/// <param name="http"><see cref="HttpClient"/> instance.</param>
public abstract class ChartHelper(HttpClient http) : IChartHelper
{
    /// <summary>
    /// Gets the <see cref="HttpClient"/> instance.
    /// </summary>
    protected HttpClient Http { get; } = http ?? throw new ArgumentNullException(nameof(http));

    /// <summary>
    /// Gets the <see cref="ISpotifyClient"/> instance.
    /// </summary>
    protected ISpotifyClient? Spotify { get; private set; }

    /// <inheritdoc />
    public abstract string Name { get; set; }

    protected async Task EnsureSpotifyClientAsync()
    {
        if (this.Spotify is not null)
        {
            return;
        }

        await this.SetSpotifyClientAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async Task<ChartItemCollection> BuildAsync(ArgumentOptions options)
    {
        await this.EnsureSpotifyClientAsync().ConfigureAwait(false);

        return new();
    }

    private async Task SetSpotifyClientAsync()
    {
        var accessToken = await this.Http.GetStringAsync("spotify/access-token").ConfigureAwait(false);
        var spotify = new SpotifyClient(accessToken);

        this.Spotify = spotify;
    }
}
