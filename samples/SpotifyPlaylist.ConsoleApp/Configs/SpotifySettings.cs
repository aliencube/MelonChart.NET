namespace SpotifyPlaylist.ConsoleApp.Configs;

/// <summary>
/// This represents the app settings entity for Spotify.
/// </summary>
public class SpotifySettings
{
    /// <summary>
    /// Define the name of the settings.
    /// </summary>
    public const string Name = "Spotify";

    /// <summary>
    /// Gets or sets the market. Default is "KR".
    /// </summary>
    public string? Market { get; set; } = "KR";

    /// <summary>
    /// Gets or sets the maximum number of items to return. Default is 10.
    /// </summary>
    public int? MaxItems { get; set; } = 10;

    /// <summary>
    /// Gets or sets the time zone ID. Default is "Asia/Seoul".
    /// </summary>
    public string? TimeZoneId { get; set; } = "Asia/Seoul";

    /// <summary>
    /// Gets or sets the <see cref="SpotifyPlaylistSettings"/> instance.
    /// </summary>
    public SpotifyPlaylistSettings? Playlist { get; set; }
}

/// <summary>
/// This represents the app settings entity for Spotify playlist.
/// </summary>
public class SpotifyPlaylistSettings
{
    /// <summary>
    /// Gets or sets the name of the playlist.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the playlist.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the cover image URL of the playlist.
    /// </summary>
    public string? CoverImageUrl { get; set; }
}
