using Blazor.SubtleCrypto;
using Blazored.SessionStorage;
using Cineder_UI.Web;
using Cineder_UI.Web.Interfaces.Services;
using Cineder_UI.Web.Models.Common;
using Cineder_UI.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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

builder.Services.Configure<CinederApiOptions>(builder.Configuration.GetSection("CinederApiOptions"));

builder.Services.Configure<SessionOptions>(builder.Configuration.GetSection("SessionOptions"));

var sessionOptions = new SessionOptions();

builder.Configuration.GetSection("SessionOptions").Bind(sessionOptions);

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddSubtleCrypto(opt =>
{
    opt.Key = sessionOptions.StoreKey;
    opt.Encryption = EncryptionType.AES_GCM;
});

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ISeriesService, SeriesService>();
builder.Services.AddScoped<IBrowserStorageService, BrowserStorageService>();

await builder.Build().RunAsync();
