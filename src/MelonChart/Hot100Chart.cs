using MelonChart.Extensions;

namespace MelonChart;

public class Hot100Chart : Chart
{
    /// <inheritdoc />
    public override ChartTypes ChartType => ChartTypes.Hot100;

    /// <inheritdoc />
    protected override async Task SetPageAsync()
    {
        if (this.Page == null)
        {
            throw new InvalidOperationException("Page is not set.");
        }

        await this.Page.GotoAsync(ChartPages.Hot100).ConfigureAwait(false);
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
