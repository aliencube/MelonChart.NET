using MelonChart.Abstractions;
using MelonChart.Extensions;
using MelonChart.Models;

using Microsoft.Playwright;

namespace MelonChart;

/// <summary>
/// This represents the top 100 chart entity.
/// </summary>
public class Top100Chart : IChart
{
    /// <inheritdoc />
    public ChartTypes ChartType => ChartTypes.Top100;

    /// <inheritdoc />
    public async Task<ChartItemCollection> GetChartAsync()
    {
        using var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
        await using var browser = await playwright.Chromium.LaunchAsync().ConfigureAwait(false);
        var page = await browser.NewPageAsync().ConfigureAwait(false);
        await page.GotoAsync(ChartPages.Top100).ConfigureAwait(false);

        var collection = new ChartItemCollection
        {
            DateLastUpdated = await page.GetTextContentAsync("span[class='year']").ToDateOnly().ConfigureAwait(false),
            TimeLastUpdated = await page.GetTextContentAsync("span[class='hour']").ToTimeOnly().ConfigureAwait(false)
        };

        var top50 = await page.Locator("tr[class='lst50']").AllAsync();
        foreach (var tr in top50)
        {
            var songId = await tr.GetAttributeAsync("data-song-no").ConfigureAwait(false);
            var rank = await tr.GetTextContentAsync("span[class='rank ']").ConfigureAwait(false);
            var title = await tr.GetTextContentAsync("div[class='ellipsis rank01']",
                                                     "span",
                                                     "a").ConfigureAwait(false);
            var artist = await tr.GetNthTextContentAsync(0, "div[class='ellipsis rank02']",
                                                            "a").ConfigureAwait(false);
            var album = await tr.GetTextContentAsync("div[class='ellipsis rank03']",
                                                     "a").ConfigureAwait(false);
            var image = await tr.GetAttributeAsync("src", "img[onerror='WEBPOCIMG.defaultAlbumImg(this);']")
                                .ConfigureAwait(false);

            collection.Items.Add(new ChartItem(songId, rank, title, artist, album, image));
        }

        var bottom50 = await page.Locator("tr[class='lst100']").AllAsync();
        foreach (var tr in bottom50)
        {
            var songId = await tr.GetAttributeAsync("data-song-no").ConfigureAwait(false);
            var rank = await tr.GetTextContentAsync("span[class='rank ']").ConfigureAwait(false);
            var title = await tr.GetTextContentAsync("div[class='ellipsis rank01']",
                                                     "span",
                                                     "a").ConfigureAwait(false);
            var artist = await tr.GetNthTextContentAsync(0, "div[class='ellipsis rank02']",
                                                            "a").ConfigureAwait(false);
            var album = await tr.GetTextContentAsync("div[class='ellipsis rank03']",
                                                     "a").ConfigureAwait(false);
            var image = await tr.GetAttributeAsync("src", "img[onerror='WEBPOCIMG.defaultAlbumImg(this);']")
                                .ConfigureAwait(false);

            collection.Items.Add(new ChartItem(songId, rank, title, artist, album, image));
        }

        return collection;
    }
}
