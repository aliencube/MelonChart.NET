using MelonChart.Extensions;

namespace MelonChart;

/// <summary>
/// This represents the top 100 chart entity.
/// </summary>
public class Top100Chart : Chart
{
    /// <inheritdoc />
    public override ChartTypes ChartType => ChartTypes.Top100;

    /// <inheritdoc />
    protected override async Task SetPageAsync()
    {
        if (this.Page == null)
        {
            throw new InvalidOperationException("Page is not set.");
        }

        await this.Page.GotoAsync(ChartPages.Top100).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override async Task SetDateTimeAsync()
    {
        if (this.Page == null)
        {
            throw new InvalidOperationException("Page is not set.");
        }

        this.Collection.DateLastUpdated = await this.Page.GetTextOfElementAsync("span[class='year']").ToDateOnly().ConfigureAwait(false);
        this.Collection.TimeLastUpdated = await this.Page.GetTextOfElementAsync("span[class='hour']").ToTimeOnly().ConfigureAwait(false);
    }
}
