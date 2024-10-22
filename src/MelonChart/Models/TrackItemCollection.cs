namespace MelonChart.Models;

/// <summary>
/// This represents the model entity for chart item collection.
/// </summary>
public class TrackItemCollection
{
    /// <summary>
    /// Gets or sets the list of <see cref="TrackItem"/> instances.
    /// </summary>
    public List<TrackItem> Items { get; set; } = [];
}