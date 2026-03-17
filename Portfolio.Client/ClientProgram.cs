using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portfolio.Client;
using Portfolio.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Set this to your API's port
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5087") });

builder.Services.AddScoped<IFileUploadService, WasmFileUploadService>();

// PortfolioService is Singleton but needs HttpClient — use factory so DI injects it correctly
builder.Services.AddSingleton<PortfolioService>(sp =>
{
    var http = sp.GetRequiredService<HttpClient>();
    return new PortfolioService(http);
});

await builder.Build().RunAsync();