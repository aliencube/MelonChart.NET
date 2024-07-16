using System.Runtime.CompilerServices;

namespace SpotifyPlaylist.ConsoleApp;

/// <summary>
/// This represents the project path information entity.
/// </summary>
/// <remarks>Reference: https://stackoverflow.com/questions/816566/how-do-you-get-the-current-project-directory-from-c-sharp-code-when-creating-a-c#answer-68864779</remarks>
internal static class ProjectPathInfo
{
    public static string CSharpClassFileName = nameof(ProjectPathInfo) + ".cs";
    public static string CSharpClassPath;
    public static string ProjectPath;

    /// <summary>
    /// Initializes static members of the <see cref="ProjectPathInfo"/> class.
    /// </summary>
    static ProjectPathInfo()
    {
        CSharpClassPath = GetSourceFilePathName();
        ProjectPath = Directory.GetParent(CSharpClassPath)!.FullName;
    }

    /// <summary>
    /// Gets the source file path name.
    /// </summary>
    /// <param name="callerFilePath">File path of the caller.</param>
    /// <returns>Returns the file path of the caller.</returns>
    private static string GetSourceFilePathName([CallerFilePath] string? callerFilePath = null) => callerFilePath ?? "";
}