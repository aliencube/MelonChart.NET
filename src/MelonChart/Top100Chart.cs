using MelonChart.Abstractions;
using MelonChart.Extensions;
using MelonChart.Models;

using Microsoft.Playwright;

namespace MelonChart;

public class Top100Chart : IChart
{
    public ChartType ChartType => ChartType.Top100;

    public async Task<ChartItemCollection> GetChartAsync()
    {
        using var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
        await using var browser = await playwright.Chromium.LaunchAsync().ConfigureAwait(false);
        var page = await browser.NewPageAsync().ConfigureAwait(false);
        await page.GotoAsync(MelonPages.Top100).ConfigureAwait(false);

        var collection = new ChartItemCollection
        {
            DateLastUpdated = await page.Locator("span[class='year']").TextContentAsync().ToDateOnly().ConfigureAwait(false),
            TimeLastUpdated = await page.Locator("span[class='hour']").TextContentAsync().ToTimeOnly().ConfigureAwait(false)
        };

        var top50 = await page.Locator("tr[class='lst50']").AllAsync();
        foreach (var tr in top50)
        {
            var songId = await tr.GetAttributeAsync("data-song-no").ConfigureAwait(false);
            var rank = await tr.Locator("span[class='rank ']").TextContentAsync().ConfigureAwait(false);
            var title = await tr.Locator("div[class='ellipsis rank01']")
                                .Locator("span")
                                .Locator("a").TextContentAsync().ConfigureAwait(false);
            var artist = await tr.Locator("div[class='ellipsis rank02']")
                                 .Locator("a").First.TextContentAsync().ConfigureAwait(false);
            var album = await tr.Locator("div[class='ellipsis rank03']")
                                .Locator("a").TextContentAsync().ConfigureAwait(false);
            var image = await tr.Locator("img[onerror='WEBPOCIMG.defaultAlbumImg(this);']")
                                .GetAttributeAsync("src").ConfigureAwait(false);

            collection.Items.Add(new ChartItem(songId, rank, title, artist, album, image));
        }

        var bottom50 = await page.Locator("tr[class='lst100']").AllAsync();
        foreach (var tr in bottom50)
        {
            var songId = await tr.GetAttributeAsync("data-song-no").ConfigureAwait(false);
            var rank = await tr.Locator("span[class='rank ']").TextContentAsync().ConfigureAwait(false);
            var title = await tr.Locator("div[class='ellipsis rank01']")
                                .Locator("span")
                                .Locator("a").TextContentAsync().ConfigureAwait(false);
            var artist = await tr.Locator("div[class='ellipsis rank02']")
                                 .Locator("a").First.TextContentAsync().ConfigureAwait(false);
            var album = await tr.Locator("div[class='ellipsis rank03']")
                                .Locator("a").TextContentAsync().ConfigureAwait(false);
            var image = await tr.Locator("img[onerror='WEBPOCIMG.defaultAlbumImg(this);']")
                                .GetAttributeAsync("src").ConfigureAwait(false);

            collection.Items.Add(new ChartItem(songId, rank, title, artist, album, image));
        }

        return collection;
    }
}
