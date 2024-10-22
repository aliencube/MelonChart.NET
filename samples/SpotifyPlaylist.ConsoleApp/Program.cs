using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SpotifyPlaylist.ConsoleApp.Configs;
using SpotifyPlaylist.ConsoleApp.Helpers;
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

                   var jso = new JsonSerializerOptions()
                   {
                       WriteIndented = true,
                       PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                       Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                       Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                   };
                   services.AddSingleton(jso);

                   services.AddHttpClient<IChartHelper, SpotifyChartHelper>(http =>
                   {
                       http.BaseAddress = new Uri(apim?.BaseUrl!);
                       http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apim?.SubscriptionKey!);
                   });
                   services.AddHttpClient<IChartHelper, MelonChartHelper>(http =>
                   {
                       http.BaseAddress = new Uri(apim?.BaseUrl!);
                       http.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apim?.SubscriptionKey!);
                   });

                   services.AddScoped<ISpotifyPlaylistService, SpotifyPlaylistService>();
               })
               .Build();

var service = host.Services.GetRequiredService<ISpotifyPlaylistService>();
await service.RunAsync(args);
