namespace SpotifyPlaylist.ConsoleApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="SpotifyPlaylistService"/> class.
/// </summary>
public interface ISpotifyPlaylistService
{
    /// <summary>
    /// Runs the application asynchronously.
    /// </summary>
    /// <param name="args">List of arguments.</param>
    Task RunAsync(string[] args);
}
