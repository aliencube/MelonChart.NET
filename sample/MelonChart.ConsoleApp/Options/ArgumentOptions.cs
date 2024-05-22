namespace MelonChart.ConsoleApp.Options;

public class ArgumentOptions
{
    public ChartType ChartType { get; set; } = ChartType.Top100;
    public bool Help { get; set; } = false;

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
                        ? Enum.TryParse<ChartType>(args[++i], ignoreCase: true, out var result)
                            ? result
                            : throw new ArgumentException("Invalid chart type. It should be 'Top100', 'Hot100', 'Daily100', 'Weekly100' or 'Monthly100'.")
                        : throw new ArgumentException("Invalid chart type. It should be 'Top100', 'Hot100', 'Daily100', 'Weekly100' or 'Monthly100'.");
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
