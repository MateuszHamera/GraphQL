using GraphQL.Client;
using GraphQL.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<TokenService>();


builder.Services
    .AddMyGraphClient()
    .ConfigureHttpClient((provider, client) =>
    {
        var token = provider.GetRequiredService<TokenService>().GetToken();

        client.BaseAddress = new Uri("https://localhost:7034/graphql");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    })
    .ConfigureWebSocketClient(client =>
    {
        client.Uri = new Uri("wss://localhost:7034/graphql");
    });


await builder.Build().RunAsync();
