namespace MelonChart.Models;

/// <summary>
/// This represents the model entity for chart item.
/// </summary>
/// <param name="SongId">Song ID.</param>
/// <param name="Rank">Rank of the song.</param>
/// <param name="Title">Title of the song.</param>
/// <param name="Artist">Artist who sings the song.</param>
/// <param name="Album">Album that the song contains.</param>
/// <param name="Image">Image URL of the song.</param>
public record ChartItem(string? SongId, string? Rank, string? Title, string? Artist, string? Album, string? Image);
