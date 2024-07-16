using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SpotifyPlaylist.ConsoleApp.Configs;
using SpotifyPlaylist.ConsoleApp.Services;

var host = Host.CreateDefaultBuilder(args)
               .UseConsoleLifetime()
               .ConfigureServices(services =>
               {
                   var config = new ConfigurationBuilder()
                                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                      .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                                      .Build();
                   var apim = config.GetSection(AzureSettings.Name).GetSection(ApiManagementSettings.Name).Get<ApiManagementSettings>();
                   var spotify = config.GetSection("Spotify").Get<SpotifySettings>();

                   services.AddSingleton(spotify!);

                   services.AddHttpClient<ISpotifyPlaylistService, SpotifyPlaylistService>((services, http) =>
                   {
                       http.BaseAddress = new Uri(apim?.BaseUrl!);
                       http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apim?.SubscriptionKey!);
                   });
               })
               .Build();

var service = host.Services.GetRequiredService<ISpotifyPlaylistService>();
await service.RunAsync(args);
