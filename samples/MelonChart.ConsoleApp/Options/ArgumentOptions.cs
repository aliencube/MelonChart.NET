namespace MelonChart.ConsoleApp.Options;

/// <summary>
/// This represents the options entity for arguments.
/// </summary>
public class ArgumentOptions
{
    /// <summary>
    /// Gets or sets the <see cref="ChartTypes"/> value.
    /// </summary>
    public ChartTypes ChartType { get; set; } = ChartTypes.Top100;

    /// <summary>
    /// Gets or sets the value indicating whether to output as JSON or not.
    /// </summary>
    public bool OutputAsJson { get; set; } = false;

    /// <summary>
    /// Gets or sets the value indicating whether to display help or not.
    /// </summary>
    public bool Help { get; set; } = false;

    /// <summary>
    /// Parses the arguments and returns the <see cref="ArgumentOptions"/> instance.
    /// </summary>
    /// <param name="args">List of arguments.</param>
    /// <returns>Returns the <see cref="ArgumentOptions"/> instance.</returns>
    public static ArgumentOptions Parse(string[] args)
    {
        var options = new ArgumentOptions();
        if (args.Length == 0)
        {
            return options;
        }

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "-c":
                case "-t":
                case "--chart":
                case "--type":
                case "--chart-type":
                    options.ChartType = i < args.Length - 1
                        ? Enum.TryParse<ChartTypes>(args[++i], ignoreCase: true, out var result)
                            ? result
                            : throw new ArgumentException("Invalid chart type. It should be 'Top100', 'Hot100', 'Daily100', 'Weekly100' or 'Monthly100'.")
                        : throw new ArgumentException("Invalid chart type. It should be 'Top100', 'Hot100', 'Daily100', 'Weekly100' or 'Monthly100'.");
                    break;

                case "--json":
                    options.OutputAsJson = true;
                    break;

                case "-h":
                case "--help":
                    options.Help = true;
                    break;
            }
        }

        return options;
    }
}
