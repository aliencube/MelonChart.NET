using MelonChart.Abstractions;
using MelonChart.Models;

using Microsoft.Playwright;

namespace MelonChart;

public class Weekly100Chart : IChart
{
    public ChartType ChartType => ChartType.Weekly100;

    public async Task<ChartItemCollection> GetChartAsync()
    {
        using var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
        await using var browser = await playwright.Chromium.LaunchAsync().ConfigureAwait(false);
        var page = await browser.NewPageAsync().ConfigureAwait(false);
        await page.GotoAsync(MelonPages.Hot100).ConfigureAwait(false);
        var html = await page.ContentAsync().ConfigureAwait(false);

        throw new NotImplementedException();
    }
}
