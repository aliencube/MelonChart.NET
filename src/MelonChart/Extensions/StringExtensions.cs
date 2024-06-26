﻿using System.Globalization;

namespace MelonChart.Extensions;

/// <summary>
/// This represents the extension entity for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    private static readonly string[] dateFormats = { "yyyy-MM-dd",
                                                     "yyyy-M-d",
                                                     "MM-dd-yyyy",
                                                     "M-d-yyyy",
                                                     "yyyy.MM.dd",
                                                     "yyyy.M.d",
                                                     "MM.dd.yyyy",
                                                     "M.d.yyyy",
                                                     "yyyy/MM/dd",
                                                     "yyyy/M/d",
                                                     "MM/dd/yyyy",
                                                     "M/d/yyyy" };

    private static readonly string[] timeFormats = { "HH:mm:ss",
                                                     "H:m:s",
                                                     "HH:mm",
                                                     "H:m",
                                                     "HH.mm",
                                                     "H.m" };

    /// <summary>
    /// Converts the date string value to given format.
    /// </summary>
    /// <param name="value">Date string value.</param>
    /// <param name="format">Date format.</param>
    /// <returns>Returns the formatted date string value.</returns>
    public static string? ToDateOnly(this string value, string? format = "yyyy-MM-dd")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        if (DateTime.TryParseExact(value, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date.ToString(format, CultureInfo.InvariantCulture);
        }

        return default;
    }

    /// <summary>
    /// Converts the date string value to given format.
    /// </summary>
    /// <param name="instance">Date string value.</param>
    /// <param name="format">Date format.</param>
    /// <returns>Returns the formatted date string value.</returns>
    public static async Task<string?> ToDateOnly(this Task<string?> instance, string? format = "yyyy-MM-dd")
    {
        var value = await instance.ConfigureAwait(false);
        return value?.ToDateOnly(format);
    }

    /// <summary>
    /// Converts the time string value to given format.
    /// </summary>
    /// <param name="value">Time string value.</param>
    /// <param name="format">Time format.</param>
    /// <returns>Returns the formatted date string value.</returns>
    public static string? ToTimeOnly(this string value, string? format = "HH:mm:ss")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        if (DateTime.TryParseExact(value, timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
        {
            return time.ToString(format, CultureInfo.InvariantCulture);
        }

        return default;
    }

    /// <summary>
    /// Converts the time string value to given format.
    /// </summary>
    /// <param name="instance">Time string value.</param>
    /// <param name="format">Time format.</param>
    /// <returns>Returns the formatted date string value.</returns>
    public static async Task<string?> ToTimeOnly(this Task<string?> instance, string? format = "HH:mm:ss")
    {
        var value = await instance.ConfigureAwait(false);
        return value?.ToTimeOnly();
    }
}
