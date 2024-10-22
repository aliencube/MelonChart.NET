namespace MelonChart.Models;

/// <summary>
/// This represents the model entity for chart item collection.
/// </summary>
public class ChartItemCollection
{
    /// <summary>
    /// Gets or sets the <see cref="ChartTypes"/> value.
    /// </summary>
    public ChartTypes ChartType { get; set; }

    /// <summary>
    /// Gets or sets the date when the chart was last updated. This value is used for both Top 100 and Hot 100 charts, as well as the Daily chart.
    /// </summary>
    public string? DateLastUpdated { get; set; }

    /// <summary>
    /// Gets or sets the time when the chart was last updated. This value is used for both Top 100 and Hot 100 charts.
    /// </summary>
    public string? TimeLastUpdated { get; set; }

    /// <summary>
    /// Gets or sets the period from when the chart was generated. This value is used for the Weekly chart.
    /// </summary>
    public string? PeriodFrom { get; set; }

    /// <summary>
    /// Gets or sets the period to when the chart was generated. This value is used for the Weekly chart.
    /// </summary>
    public string? PeriodTo { get; set; }

    /// <summary>
    /// Gets or sets the year of the chart. This value is used for the Monthly chart.
    /// </summary>
    public string? Year { get; set; }

    /// <summary>
    /// Gets or sets the month of the chart. This value is used for the Monthly chart.
    /// </summary>
    public string? Month { get; set; }

    /// <summary>
    /// Gets or sets the list of <see cref="ChartItem"/> instances that are listed on the Melon chart.
    /// </summary>
    public List<ChartItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of <see cref="TrackItem"/> instances that are missing from the Spotify chart.
    /// </summary>
    public List<TrackItem> MissingTracks { get; set; } = [];
}