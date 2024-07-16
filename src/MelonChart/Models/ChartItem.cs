﻿namespace MelonChart.Models;

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
    public int? Rank { get; set; }

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

    /// <summary>
    /// Gets or sets the track ID from Spotify.
    /// </summary>
    public string? TrackId { get; set; }

    /// <summary>
    /// Gets or sets the track URI from Spotify.
    /// </summary>
    public string? TrackUri { get; set; }

    /// <summary>
    /// Gets or sets the danceability of the song from Spotify. 0 is the least danceable and 1 is the most danceable.
    /// </summary>
    public float? Danceability { get; set; }

    /// <summary>
    /// Gets or sets the valence of the song from Spotify. Close to 0 is more negative and close to 1 is more positive.
    /// </summary>
    public float? Valence { get; set; }
}
