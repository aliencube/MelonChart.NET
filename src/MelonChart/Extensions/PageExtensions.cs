using Microsoft.Playwright;

namespace MelonChart.Extensions;

/// <summary>
/// This represents the helper entity for <see cref="Page"/>
/// </summary>
public static class PageExtensions
{
    /// <summary>
    /// Gets the value from the given attribute of the element.
    /// </summary>
    /// <param name="page"><see cref="IPage"/> instance.</param>
    /// <param name="name">Name of the attribute.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the attribute value.</returns>
    public static async Task<string?> GetAttributeAsync(this IPage page, string name, params string[] selectors)
    {
        if (selectors.Length == 0)
        {
            return await page.GetAttributeAsync(name).ConfigureAwait(false);
        }
        if (selectors.Length == 1)
        {
            return await page.Locator(selectors[0]).GetAttributeAsync(name).ConfigureAwait(false);
        }

        var text = page.Locator(selectors[0]);
        return await text.GetAttributeAsync(name, selectors[1..]).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the value from the given attribute of the element.
    /// </summary>
    /// <param name="page"><see cref="IPage"/> instance.</param>
    /// <param name="name">Name of the attribute.</param>
    /// <param name="index">Index of the element.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the attribute value.</returns>
    public static async Task<string?> GetNthAttributeAsync(this IPage page, string name, int index = 0, params string[] selectors)
    {
        if (selectors.Length == 0)
        {
            return await page.GetAttributeAsync(name).ConfigureAwait(false);
        }
        if (selectors.Length == 1)
        {
            return await page.Locator(selectors[0]).Nth(index).GetAttributeAsync(name).ConfigureAwait(false);
        }

        var text = page.Locator(selectors[0]);
        return await text.GetNthAttributeAsync(name, index, selectors[1..]).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the value from the given element.
    /// </summary>
    /// <param name="page"><see cref="IPage"/> instance.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the element value.</returns>
    public static async Task<string?> GetTextContentAsync(this IPage page, params string[] selectors)
    {
        if (selectors.Length == 0)
        {
            return default;
        }
        if (selectors.Length == 1)
        {
            return await page.Locator(selectors[0]).TextContentAsync().ConfigureAwait(false);
        }

        var text = page.Locator(selectors[0]);
        return await text.GetTextContentAsync(selectors[1..]).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the value from the given element.
    /// </summary>
    /// <param name="page"><see cref="IPage"/> instance.</param>
    /// <param name="index">Index of the element.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the element value.</returns>
    public static async Task<string?> GetNthTextContentAsync(this IPage page, int index = 0, params string[] selectors)
    {
        if (selectors.Length == 0)
        {
            return default;
        }
        if (selectors.Length == 1)
        {
            return await page.Locator(selectors[0]).Nth(index).TextContentAsync().ConfigureAwait(false);
        }

        var text = page.Locator(selectors[0]);
        return await text.GetNthTextContentAsync(index, selectors[1..]).ConfigureAwait(false);
    }
}