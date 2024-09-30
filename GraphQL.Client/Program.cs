using GraphQL.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddMyGraphClient()
    .ConfigureHttpClient(client => 
    client.BaseAddress = new Uri("https://localhost:7034/graphql"))
    .ConfigureWebSocketClient(client => 
    client.Uri = new Uri("wss://localhost:7034/graphql")); 

await builder.Build().RunAsync();
