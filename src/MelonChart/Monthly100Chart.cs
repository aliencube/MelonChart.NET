using MelonChart.Abstractions;
using MelonChart.Extensions;
using MelonChart.Models;

using Microsoft.Playwright;

namespace MelonChart;

public class Monthly100Chart : Chart
{
    /// <inheritdoc />
    public override ChartTypes ChartType => ChartTypes.Monthly100;

    /// <inheritdoc />
    protected override async Task SetPageAsync()
    {
        if (this.Page == null)
        {
            throw new InvalidOperationException("Page is not set.");
        }

        await this.Page.GotoAsync(ChartPages.Monthly100).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override async Task SetDateTimeAsync()
    {
        if (this.Page == null)
        {
            throw new InvalidOperationException("Page is not set.");
        }

        var period = await this.Page.GetTextOfElementAsync("span[class='yyyymmdd']").ConfigureAwait(false);
        var segments = period?.Split(".");
        this.Collection.Year = segments?.First().Trim();
        this.Collection.Month = segments?.Last().Trim();
    }
}
