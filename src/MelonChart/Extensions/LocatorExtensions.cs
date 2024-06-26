using Microsoft.Playwright;

namespace MelonChart.Extensions;

/// <summary>
/// This represents the helper entity for <see cref="Locator"/>
/// </summary>
public static class LocatorExtensions
{
    /// <summary>
    /// Gets the value from the given attribute of the element.
    /// </summary>
    /// <param name="locator"><see cref="ILocator"/> instance.</param>
    /// <param name="name">Name of the attribute.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the attribute value.</returns>
    public static async Task<string?> GetAttributeOfElementAsync(this ILocator? locator, string name, params string[] selectors)
    {
        if (locator == null)
        {
            throw new ArgumentNullException(nameof(locator));
        }

        var text = locator;
        foreach (var selector in selectors)
        {
            text = text.Locator(selector);
        }

        var value = await text.GetAttributeAsync(name).ConfigureAwait(false);

        return value;
    }

    /// <summary>
    /// Gets the value from the given attribute of the element.
    /// </summary>
    /// <param name="locator"><see cref="ILocator"/> instance.</param>
    /// <param name="name">Name of the attribute.</param>
    /// <param name="index">Index of the element.</param>
    /// <param name="useFallbackValue">Value indicating whether to use the fallback value or not.</param>
    /// <param name="fallbackValue">Fallback value.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the attribute value.</returns>
    public static async Task<string?> GetAttributeOfNthElementAsync(this ILocator? locator, string name, int index = 0, bool useFallbackValue = false, string? fallbackValue = null, params string[] selectors)
    {
        if (locator == null)
        {
            throw new ArgumentNullException(nameof(locator));
        }

        var text = locator;
        foreach (var selector in selectors)
        {
            text = text.Locator(selector);
        }

        string? value = default;
        try
        {
            value = await text.Nth(index).GetAttributeAsync(name).ConfigureAwait(false);
        }
        catch
        {
            if (useFallbackValue)
            {
                value = fallbackValue;
            }
            else
            {
                throw;
            }
        }

        return value;
    }

    /// <summary>
    /// Gets the value from the given element.
    /// </summary>
    /// <param name="locator"><see cref="ILocator"/> instance.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the element value.</returns>
    public static async Task<string?> GetTextOfElementAsync(this ILocator? locator, params string[] selectors)
    {
        if (locator == null)
        {
            throw new ArgumentNullException(nameof(locator));
        }

        var text = locator;
        foreach (var selector in selectors)
        {
            text = text.Locator(selector);
        }

        var value = await text.TextContentAsync().ConfigureAwait(false);

        return value;
    }

    /// <summary>
    /// Gets the value from the given element.
    /// </summary>
    /// <param name="locator"><see cref="ILocator"/> instance.</param>
    /// <param name="index">Index of the element.</param>
    /// <param name="useFallbackValue">Value indicating whether to use the fallback value or not.</param>
    /// <param name="fallbackValue">Fallback value.</param>
    /// <param name="selectors">List of selectors.</param>
    /// <returns>Returns the element value.</returns>
    public static async Task<string?> GetTextOfNthElementAsync(this ILocator? locator, int index = 0, bool useFallbackValue = false, string? fallbackValue = null, params string[] selectors)
    {
        if (locator == null)
        {
            throw new ArgumentNullException(nameof(locator));
        }

        var text = locator;
        foreach (var selector in selectors)
        {
            text = text.Locator(selector);
        }

        string? value = default;
        try
        {
            value = await text.Nth(index).TextContentAsync().ConfigureAwait(false);
        }
        catch
        {
            if (useFallbackValue)
            {
                value = fallbackValue;
            }
            else
            {
                throw;
            }
        }

        return value;
    }
}