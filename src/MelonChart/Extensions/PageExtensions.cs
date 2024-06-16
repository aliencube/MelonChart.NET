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
    public static async Task<string?> GetAttributeOfElementAsync(this IPage? page, string name, params string[] selectors)
    {
        if (page == null)
        {
            throw new ArgumentNullException(nameof(page));
        }

        if (selectors.Length == 0)
        {
            throw new ArgumentException("Selectors must be provided", nameof(selectors));
        }

        if (selectors.Length == 1)
        {
            return await page.Locator(selectors[0]).GetAttributeAsync(name).ConfigureAwait(false);
        }

        var text = page.Locator(selectors[0]);

        return await text.GetAttributeOfElementAsync(name, selectors[1..]).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the value from the given attribute of the element.
    /// </summary>
    /// <param name="page"><see cref="IPage"/> instance.</param>
    /// <param name="name">Name of the attribute.</param>
    /// <param name="index">Index of the element.</param>
    /// <param name="useFallbackValue">Value indicating whether to use the fallback value or not.</param>
    /// <param name="fallbackValue">Fallback value.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the attribute value.</returns>
    public static async Task<string?> GetAttributeOfNthElementAsync(this IPage? page, string name, int index = 0, bool useFallbackValue = false, string? fallbackValue = null, params string[] selectors)
    {
        if (page == null)
        {
            throw new ArgumentNullException(nameof(page));
        }

        if (selectors.Length == 0)
        {
            throw new ArgumentException("Selectors must be provided", nameof(selectors));
        }

        if (selectors.Length == 1)
        {
            return await page.Locator(selectors[0]).Nth(index).GetAttributeAsync(name).ConfigureAwait(false);
        }

        var text = page.Locator(selectors[0]);

        return await text.GetAttributeOfNthElementAsync(name, index, useFallbackValue, fallbackValue, selectors[1..]).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the value from the given element.
    /// </summary>
    /// <param name="page"><see cref="IPage"/> instance.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the element value.</returns>
    public static async Task<string?> GetTextOfElementAsync(this IPage? page, params string[] selectors)
    {
        if (page == null)
        {
            throw new ArgumentNullException(nameof(page));
        }

        if (selectors.Length == 0)
        {
            throw new ArgumentException("Selectors must be provided", nameof(selectors));
        }

        if (selectors.Length == 1)
        {
            return await page.Locator(selectors[0]).TextContentAsync().ConfigureAwait(false);
        }

        var text = page.Locator(selectors[0]);

        return await text.GetTextOfElementAsync(selectors[1..]).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the value from the given element.
    /// </summary>
    /// <param name="page"><see cref="IPage"/> instance.</param>
    /// <param name="index">Index of the element.</param>
    /// <param name="useFallbackValue">Value indicating whether to use the fallback value or not.</param>
    /// <param name="fallbackValue">Fallback value.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the element value.</returns>
    public static async Task<string?> GetTextOfNthElementAsync(this IPage? page, int index = 0, bool useFallbackValue = false, string? fallbackValue = null, params string[] selectors)
    {
        if (page == null)
        {
            throw new ArgumentNullException(nameof(page));
        }

        if (selectors.Length == 0)
        {
            throw new ArgumentException("Selectors must be provided", nameof(selectors));
        }

        if (selectors.Length == 1)
        {
            return await page.Locator(selectors[0]).Nth(index).TextContentAsync().ConfigureAwait(false);
        }

        var text = page.Locator(selectors[0]);

        return await text.GetTextOfNthElementAsync(index, useFallbackValue, fallbackValue, selectors[1..]).ConfigureAwait(false);
    }
}