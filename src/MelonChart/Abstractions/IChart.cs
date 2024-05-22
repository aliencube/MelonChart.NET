using MelonChart.Models;

namespace MelonChart.Abstractions;

public interface IChart
{
    ChartType ChartType { get; }

    Task<ChartItemCollection> GetChartAsync();
}