namespace MelonChart.Models;

/// <summary>
/// This represents the model entity for chart item.
/// </summary>
public class ChartItem
{
    /// <summary>
    /// Gets or sets the song ID.
    /// </summary>
    public string? SongId { get; set; }

    /// <summary>
    /// Gets or sets the rank of the song.
    /// </summary>
    public string? Rank { get; set; }

    /// <summary>
    /// Gets or sets the rank up or down.
    /// </summary>
    public RankStatus RankStatus { get; set; }

    /// <summary>
    /// Gets or sets the rank up or down value.
    /// </summary>
    public int? RankStatusValue { get; set; }

    /// <summary>
    /// Gets or sets the title of the song.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the artist who sings the song.
    /// </summary>
    public string? Artist { get; set; }

    /// <summary>
    /// Gets or sets the album that the song contains.
    /// </summary>
    public string? Album { get; set; }

    /// <summary>
    /// Gets or sets the image URL of the song.
    /// </summary>
    public string? Image { get; set; }
}
