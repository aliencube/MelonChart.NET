using MelonChart.Extensions;

namespace MelonChart;

public class Weekly100Chart : Chart
{
    /// <inheritdoc />
    public override ChartTypes ChartType => ChartTypes.Weekly100;

    /// <inheritdoc />
    protected override async Task SetPageAsync()
    {
        if (this.Page == null)
        {
            throw new InvalidOperationException("Page is not set.");
        }

        await this.Page.GotoAsync(ChartPages.Weekly100).ConfigureAwait(false);
    }

    /// <inheritdoc />
    protected override async Task SetDateTimeAsync()
    {
        if (this.Page == null)
        {
            throw new InvalidOperationException("Page is not set.");
        }

        var period = await this.Page.GetTextOfElementAsync("span[class='yyyymmdd']").ConfigureAwait(false);
        var segments = period?.Split("~");
        this.Collection.PeriodFrom = segments?.First().Trim().ToDateOnly();
        this.Collection.PeriodTo = segments?.Last().Trim().ToDateOnly();
    }
}
