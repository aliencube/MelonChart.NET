namespace MelonChart.Extensions;

public static class StringExtensions
{
    public static DateOnly? ToDateOnly(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        if (DateOnly.TryParse(value, out var date))
        {
            return date;
        }

        return default;
    }

    public static async Task<DateOnly?> ToDateOnly(this Task<string?> instance)
    {
        var value = await instance.ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        if (DateOnly.TryParse(value, out var date))
        {
            return date;
        }

        return default;
    }

    public static TimeOnly? ToTimeOnly(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        if (TimeOnly.TryParse(value, out var time))
        {
            return time;
        }

        return default;
    }

    public static async Task<TimeOnly?> ToTimeOnly(this Task<string?> instance)
    {
        var value = await instance.ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        if (TimeOnly.TryParse(value, out var time))
        {
            return time;
        }

        return default;
    }
}
