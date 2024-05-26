using MelonChart;
using MelonChart.Abstractions;
using MelonChart.ConsoleApp.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
               .UseConsoleLifetime()
               .ConfigureServices(services =>
               {
                   services.AddTransient<IChart, Top100Chart>();
                   services.AddTransient<IChart, Hot100Chart>();
                   services.AddTransient<IChart, Daily100Chart>();
                   services.AddTransient<IChart, Weekly100Chart>();
                   services.AddTransient<IChart, Monthly100Chart>();
                   services.AddTransient<IMelonChartService, MelonChartService>();
               })
               .Build();

var service = host.Services.GetRequiredService<IMelonChartService>();
await service.RunAsync(args);
