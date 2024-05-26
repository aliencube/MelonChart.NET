using MelonChart;
using MelonChart.Abstractions;
using MelonChart.WebApp.Components;
using MelonChart.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IChart, Top100Chart>();
builder.Services.AddScoped<IChart, Hot100Chart>();
builder.Services.AddScoped<IChart, Daily100Chart>();
builder.Services.AddScoped<IChart, Weekly100Chart>();
builder.Services.AddScoped<IChart, Monthly100Chart>();
builder.Services.AddScoped<IMelonChartService, MelonChartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
