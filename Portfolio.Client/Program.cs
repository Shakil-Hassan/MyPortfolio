using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portfolio.Client;
using Portfolio.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// IMPORTANT: Set this to your API's port (5087)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5087") });

builder.Services.AddScoped<IFileUploadService, WasmFileUploadService>();
builder.Services.AddSingleton<PortfolioService>();

await builder.Build().RunAsync();