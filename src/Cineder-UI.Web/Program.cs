using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Cineder_UI.Web;
using Cineder_UI.Web.Interfaces.Services;
using Cineder_UI.Web.Services;
using Cineder_UI.Web.Models.Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var httpClient = new HttpClient()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};

builder.Services.AddScoped(sp => httpClient);

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;

var configName = string.IsNullOrWhiteSpace(env) ? "appsettings.json" : $"appsettings.{env}.json";

using var configResponse = await httpClient.GetAsync(configName);

using var configStream = await configResponse.Content.ReadAsStreamAsync();

builder.Configuration.AddJsonStream(configStream);

var cinederApiOptions = builder.Configuration.GetSection("CinederApiOptions");

builder.Services.Configure<CinederApiOptions>(cinederApiOptions);

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ISeriesService, SeriesService>();

await builder.Build().RunAsync();
