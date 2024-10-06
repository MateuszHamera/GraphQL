using Microsoft.JSInterop;

namespace GraphQL.Client.Services;

public class TokenService
{
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";

    public TokenService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    // Synchronous method to get token
    public string GetToken()
    {
        // Synchronously retrieve the token using Task.Run
        return Task.Run(async () =>
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        }).Result;
    }

    public async Task SetTokenAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
    }
}
