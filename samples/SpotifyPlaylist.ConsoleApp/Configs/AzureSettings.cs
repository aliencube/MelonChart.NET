namespace SpotifyPlaylist.ConsoleApp.Configs;

/// <summary>
/// This represents the app settings entity for Azure.
/// </summary>
public class AzureSettings
{
    /// <summary>
    /// Define the name of the settings.
    /// </summary>
    public const string Name = "Azure";

    /// <summary>
    /// Gets or sets the <see cref="ApiManagementSettings"/> instance.
    /// </summary>
    public ApiManagementSettings? APIM { get; set; }
}

/// <summary>
/// This represents the app settings entity for API Management.
/// </summary>
public class ApiManagementSettings
{
    /// <summary>
    /// Define the name of the settings.
    /// </summary>
    public const string Name = "APIM";

    /// <summary>
    /// Gets or sets the base URL.
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the subscription key.
    /// </summary>
    public string? SubscriptionKey { get; set; }
}
