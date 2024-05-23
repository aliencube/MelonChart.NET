using MelonChart.Abstractions;
using MelonChart.Models;

using Microsoft.Playwright;

namespace MelonChart;

public class Hot100Chart : IChart
{
    public ChartTypes ChartType => ChartTypes.Hot100;

    public async Task<ChartItemCollection> GetChartAsync()
    {
        using var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
        await using var browser = await playwright.Chromium.LaunchAsync().ConfigureAwait(false);
        var page = await browser.NewPageAsync().ConfigureAwait(false);
        await page.GotoAsync(ChartPages.Hot100).ConfigureAwait(false);
        var html = await page.ContentAsync().ConfigureAwait(false);

        throw new NotImplementedException();
    }
}
