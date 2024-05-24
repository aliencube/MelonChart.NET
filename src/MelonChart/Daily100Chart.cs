using MelonChart.Abstractions;
using MelonChart.Extensions;
using MelonChart.Models;

using Microsoft.Playwright;

namespace MelonChart;

public class Daily100Chart : Chart
{
    /// <inheritdoc />
    public override ChartTypes ChartType => ChartTypes.Daily100;

    /// <inheritdoc />
    protected override async Task SetPageAsync()
    {
        if (this.Page == null)
        {
            throw new InvalidOperationException("Page is not set.");
        }

        await this.Page.GotoAsync(ChartPages.Daily100).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override async Task SetDateTimeAsync()
    {
        if (this.Page == null)
        {
            throw new InvalidOperationException("Page is not set.");
        }

        this.Collection.DateLastUpdated = await this.Page.GetTextOfElementAsync("span[class='year']").ToDateOnly().ConfigureAwait(false);
    }
}
