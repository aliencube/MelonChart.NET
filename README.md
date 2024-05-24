# MelonChart.NET

This is the Melon chart scraping library written in .NET - Top 100, Hot 100, Daily, Weekly and Monthly

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) 17.0+ or [Visual Studio Code](https://code.visualstudio.com/) 1.80+ with [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

## Getting Started

1. Install the NuGet package of this library.

```bash
dotnet add package MelonChart.NET
```

1. Use the library in your code.

```csharp
var chart = new Top100Chart();
var collection = await chart.GetChartAsync();
foreach (var item in collection.Items)
{
    Console.WriteLine($"{item.Rank} - {item.Title} by {item.Artist}");
}
```

1. If you want to get the Hot 100 chart, use the `Hot100Chart` class.

```csharp
var chart = new Hot100Chart();
var collection = await chart.GetChartAsync();
foreach (var item in collection.Items)
{
    Console.WriteLine($"{item.Rank} - {item.Title} by {item.Artist}");
}
```

1. You can also register all the charts and get the chart by the type.

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddKeyedScoped<IChart, Top100Chart>(ChartTypes.Top100);
builder.Services.AddKeyedScoped<IChart, Hot100Chart>(ChartTypes.Hot100);
builder.Services.AddKeyedScoped<IChart, Daily100Chart>(ChartTypes.Daily100);
builder.Services.AddKeyedScoped<IChart, Weekly100Chart>(ChartTypes.Weekly100);
builder.Services.AddKeyedScoped<IChart, Monthly100Chart>(ChartTypes.Monthly100);

var app = builder.Build();

app.MapGet("/top100", async ([FromKeyedServices(ChartTypes.Top100)] IChart chart) =>
{
    var collection = await chart.GetChartAsync();
    return Results.Json(collection.Select(p => new { p.Rank, p.Title, p.Artist }));
});

app.MapGet("/hot100", async ([FromKeyedServices(ChartTypes.Hot100)] IChart chart) =>
{
    var collection = await chart.GetChartAsync();
    return Results.Json(collection.Select(p => new { p.Rank, p.Title, p.Artist }));
});

app.MapGet("/daily100", async ([FromKeyedServices(ChartTypes.Daily100)] IChart chart) =>
{
    var collection = await chart.GetChartAsync();
    return Results.Json(collection.Select(p => new { p.Rank, p.Title, p.Artist }));
});

app.MapGet("/weekly100", async ([FromKeyedServices(ChartTypes.Weekly100)] IChart chart) =>
{
    var collection = await chart.GetChartAsync();
    return Results.Json(collection.Select(p => new { p.Rank, p.Title, p.Artist }));
});

app.MapGet("/monthly100", async ([FromKeyedServices(ChartTypes.Monthly100)] IChart chart) =>
{
    var collection = await chart.GetChartAsync();
    return Results.Json(collection.Select(p => new { p.Rank, p.Title, p.Artist }));
});
```
