namespace MelonChart.ConsoleApp.Services;

/// <summary>
/// This provides interfaces to the <see cref="MelonChartService"/> class.
/// </summary>
public interface IMelonChartService
{
    /// <summary>
    /// Runs the application asynchronously.
    /// </summary>
    /// <param name="args">List of arguments.</param>
    Task RunAsync(string[] args);
}
