namespace GraphQL.Server.Payloads;

public class LoginPayload
{
    public string Token { get; set; }

    public LoginPayload(string token)
    {
        Token = token;
    }
}
