using GraphQL.Client;
using GraphQL.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<TokenService>();


builder.Services
    .AddMyGraphClient()
    .ConfigureHttpClient((sp, client) =>
    {
        // Synchronously get the token from the TokenService
        var tokenService = sp.GetRequiredService<TokenService>();
        var token = tokenService.GetToken();

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        client.BaseAddress = new Uri("https://localhost:7034/graphql");
    })
    .ConfigureWebSocketClient(client =>
    {
        client.Uri = new Uri("wss://localhost:7034/graphql");
    });


await builder.Build().RunAsync();
