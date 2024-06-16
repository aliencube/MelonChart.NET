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
        this.Collection.Items.AddRange(await this.GetChartItemsAsync(top50).ConfigureAwait(false));

        var bottom50 = await this.Page.Locator("tr[class='lst100']").AllAsync();
        this.Collection.Items.AddRange(await this.GetChartItemsAsync(bottom50).ConfigureAwait(false));

        return this.Collection;
    }

    private async Task<List<ChartItem>> GetChartItemsAsync(IEnumerable<ILocator> locators)
    {
        var items = new List<ChartItem>();
        foreach (var locator in locators)
        {
            var rankStatus = await locator.GetAttributeOfNthElementAsync("class", 2,
                                                                         useFallbackValue: true, fallbackValue: "new",
                                                                         selectors: [ "span[class='rank_wrap']",
                                                                                      "span" ]).ConfigureAwait(false);
            var rankStatusValue = await locator.GetTextOfNthElementAsync(2,
                                                                         useFallbackValue: true, fallbackValue: "0",
                                                                         selectors: [ "span[class='rank_wrap']",
                                                                                      "span" ]).ConfigureAwait(false);
            var songId = await locator.GetAttributeOfElementAsync("data-song-no").ConfigureAwait(false);
            var rank = await locator.GetTextOfElementAsync("span[class='rank ']").ConfigureAwait(false);
            var title = await locator.GetTextOfElementAsync("div[class='ellipsis rank01']",
                                                            "span",
                                                            "a").ConfigureAwait(false);
            var artist = await locator.GetTextOfNthElementAsync(0, selectors: [ "div[class='ellipsis rank02']",
                                                                                "a" ]).ConfigureAwait(false);
            var album = await locator.GetTextOfElementAsync("div[class='ellipsis rank03']",
                                                            "a").ConfigureAwait(false);
            var image = await locator.GetAttributeOfElementAsync("src", "img[onerror='WEBPOCIMG.defaultAlbumImg(this);']")
                                     .ConfigureAwait(false);

            items.Add(new ChartItem()
                      {
                          SongId = songId,
                          Rank = rank,
                          RankStatus = Enum.TryParse<RankStatus>(rankStatus, ignoreCase: true, out var result) ? result : RankStatus.Undefined,
                          RankStatusValue = Convert.ToInt32(rankStatusValue),
                          Title = title,
                          Artist = artist,
                          Album = album,
                          Image = image,
                      });
        }

        return items;
    }
}
