namespace GraphQL.Client.Services;

public class TokenService
{
    private string _token;

    // Asynchronous method to get the token
    public string GetToken()
    {
        return _token ?? string.Empty;
    }

    public async Task SetToken(string token)
    {
        _token = token;
    }
}

