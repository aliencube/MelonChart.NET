using MelonChart.Abstractions;
using MelonChart.Models;

namespace MelonChart.WebApp.Services;

/// <summary>
/// This represents the service entity for Melon chart.
/// </summary>
/// <param name="charts">List of <see cref="IChart"/> instances.</param>
public class MelonChartService(IEnumerable<IChart> charts) : IMelonChartService
{
    private readonly IEnumerable<IChart> _charts = charts ?? throw new ArgumentNullException(nameof(charts));

    /// <inheritdoc />
    public async Task<ChartItemCollection> RunAsync(ChartTypes? chartType = ChartTypes.Top100)
    {
        var chart = this._charts.SingleOrDefault(p => p.ChartType.Equals(chartType));
        if (chart is null)
        {
            throw new ArgumentException("Invalid chart type. It should be 'Top100', 'Hot100', 'Daily100', 'Weekly100' or 'Monthly100'.");
        }

        var collection = await chart.GetChartAsync().ConfigureAwait(false);

        return collection;
    }
}
