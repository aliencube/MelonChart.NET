namespace MelonChart.Models;

public class ChartItemCollection
{
    public DateOnly? DateLastUpdated { get; set; }
    public TimeOnly? TimeLastUpdated { get; set; }
    public DateTime? PeriodFrom { get; set; }
    public DateTime? PeriodTo { get; set; }
    public DateOnly? Month { get; set; }

    public List<ChartItem> Items { get; set; } = [];
}