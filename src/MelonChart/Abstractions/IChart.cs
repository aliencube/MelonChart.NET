using MelonChart.Models;

namespace MelonChart.Abstractions;

/// <summary>
/// This represents a chart interface.
/// </summary>
public interface IChart
{
    /// <summary>
    /// Gets the <see cref="ChartTypes"/> value.
    /// </summary>
    ChartTypes ChartType { get; }

    /// <summary>
    /// Gets the chart.
    /// </summary>
    /// <returns>Returns the <see cref="ChartItemCollection"/> instance.</returns>
    Task<ChartItemCollection> GetChartAsync();
}
