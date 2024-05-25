using MelonChart.Abstractions;
using MelonChart.Extensions;
using MelonChart.Models;

using Microsoft.Playwright;

namespace MelonChart;

/// <summary>
/// This represents the chart entity. This must be inherited.
/// </summary>
public abstract class Chart : IChart
{
    /// <summary>
    /// Gets the <see cref="IPage"/> instance.
    /// </summary>
    protected IPage? Page { get; private set; }

    /// <summary>
    /// Gets the <see cref="ChartItemCollection"/> instance.
    /// </summary>
    protected ChartItemCollection Collection { get; } = new();

    /// <inheritdoc />
    public abstract ChartTypes ChartType { get; }

    /// <summary>
    /// Sets the page by filling in the content.
    /// </summary>
    protected abstract Task SetPageAsync();

    /// <summary>
    /// Sets the date and time of the chart.
    /// </summary>
    /// <returns></returns>
    protected abstract Task SetDateTimeAsync();

    /// <inheritdoc />
    public async Task<ChartItemCollection> GetChartAsync()
    {
        using var playwright = await Playwright.CreateAsync().ConfigureAwait(false);
        await using var browser = await playwright.Chromium.LaunchAsync().ConfigureAwait(false);

        this.Page = await browser.NewPageAsync().ConfigureAwait(false);

        await this.SetPageAsync();

        this.Collection.ChartType = this.ChartType;

        await this.SetDateTimeAsync();

        var top50 = await this.Page.Locator("tr[class='lst50']").AllAsync();
        foreach (var tr in top50)
        {
            var songId = await tr.GetAttributeOfElementAsync("data-song-no").ConfigureAwait(false);
            var rank = await tr.GetTextOfElementAsync("span[class='rank ']").ConfigureAwait(false);
            var title = await tr.GetTextOfElementAsync("div[class='ellipsis rank01']",
                                                       "span",
                                                       "a").ConfigureAwait(false);
            var artist = await tr.GetTextOfNthElementAsync(0, "div[class='ellipsis rank02']",
                                                              "a").ConfigureAwait(false);
            var album = await tr.GetTextOfElementAsync("div[class='ellipsis rank03']",
                                                       "a").ConfigureAwait(false);
            var image = await tr.GetAttributeOfElementAsync("src", "img[onerror='WEBPOCIMG.defaultAlbumImg(this);']")
                                .ConfigureAwait(false);

            this.Collection.Items.Add(new ChartItem(songId, rank, title, artist, album, image));
        }

        var bottom50 = await this.Page.Locator("tr[class='lst100']").AllAsync();
        foreach (var tr in bottom50)
        {
            var songId = await tr.GetAttributeOfElementAsync("data-song-no").ConfigureAwait(false);
            var rank = await tr.GetTextOfElementAsync("span[class='rank ']").ConfigureAwait(false);
            var title = await tr.GetTextOfElementAsync("div[class='ellipsis rank01']",
                                                       "span",
                                                       "a").ConfigureAwait(false);
            var artist = await tr.GetTextOfNthElementAsync(0, "div[class='ellipsis rank02']",
                                                              "a").ConfigureAwait(false);
            var album = await tr.GetTextOfElementAsync("div[class='ellipsis rank03']",
                                                       "a").ConfigureAwait(false);
            var image = await tr.GetAttributeOfElementAsync("src", "img[onerror='WEBPOCIMG.defaultAlbumImg(this);']")
                                .ConfigureAwait(false);

            this.Collection.Items.Add(new ChartItem(songId, rank, title, artist, album, image));
        }

        return this.Collection;
    }
}
