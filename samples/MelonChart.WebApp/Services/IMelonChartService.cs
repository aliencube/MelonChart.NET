using MelonChart.Models;

namespace MelonChart.WebApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="MelonChartService"/> class.
/// </summary>
public interface IMelonChartService
{
    /// <summary>
    /// Runs the application asynchronously.
    /// </summary>
    /// <param name="chartType"><see cref="ChartTypes"/> value.</param>
    /// <returns>Returns the <see cref="ChartItemCollection"/> instance.</returns>
    Task<ChartItemCollection> RunAsync(ChartTypes? chartType = ChartTypes.Top100);
}
