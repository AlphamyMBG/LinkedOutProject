

namespace BackendApp.Auth;


public class TokenResponse
(string token, ulong id)
{
    public string Token{get; set;} = token;
    public ulong Id{get; set;} = id;
}