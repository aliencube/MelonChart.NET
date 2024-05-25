namespace MelonChart.Models;

/// <summary>
/// This represents the model entity for chart item.
/// </summary>
/// <param name="songId">Song ID.</param>
/// <param name="rank">Rank of the song.</param>
/// <param name="title">Title of the song.</param>
/// <param name="artist">Artist who sings the song.</param>
/// <param name="album">Album that the song contains.</param>
/// <param name="image">Image URL of the song.</param>
public class ChartItem(string? songId, string? rank, string? title, string? artist, string? album, string? image)
{
    /// <summary>
    /// Gets the song ID.
    /// </summary>
    public string? SongId { get; } = songId;

    /// <summary>
    /// Gets the rank of the song.
    /// </summary>
    public string? Rank { get; } = rank;

    /// <summary>
    /// Gets the title of the song.
    /// </summary>
    public string? Title { get; } = title;

    /// <summary>
    /// Gets the artist who sings the song.
    /// </summary>
    public string? Artist { get; } = artist;

    /// <summary>
    /// Gets the album that the song contains.
    /// </summary>
    public string? Album { get; } = album;

    /// <summary>
    /// Gets the image URL of the song.
    /// </summary>
    public string? Image { get; } = image;
}
