namespace MelonChart.Models;

public class ChartItemCollection
{
    public string? DateLastUpdated { get; set; }
    public string? TimeLastUpdated { get; set; }
    public string? PeriodFrom { get; set; }
    public string? PeriodTo { get; set; }
    public string? Year { get; set; }
    public string? Month { get; set; }

    public List<ChartItem> Items { get; set; } = [];
}