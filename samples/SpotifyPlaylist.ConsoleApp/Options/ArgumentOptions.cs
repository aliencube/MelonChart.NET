namespace SpotifyPlaylist.ConsoleApp.Options;

/// <summary>
/// This represents the options entity for arguments.
/// </summary>
public class ArgumentOptions
{
    /// <summary>
    /// Gets or sets the source that contains the Melon Chart data.
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the source type.
    /// </summary>
    public SourceType SourceType { get; set; }

    /// <summary>
    /// Gets or sets the query to search for.
    /// </summary>
    public string? Query { get; set; }

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
                case "-t":
                case "--source-type":
                    if (i < args.Length - 1)
                    {
                        options.SourceType = Enum.TryParse<SourceType>(args[++i], ignoreCase: true, out var result)
                            ? result
                            : SourceType.Undefined;
                    }
                    break;

                case "-s":
                case "--source":
                    if (i < args.Length - 1)
                    {
                        options.Source = args[++i];
                    }
                    break;

                case "-q":
                case "--query":
                    if (i < args.Length - 1)
                    {
                        options.Query = args[++i];
                    }
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
