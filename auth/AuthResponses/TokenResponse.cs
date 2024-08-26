

using BackendApp.Model.Enums;

namespace BackendApp.Auth;

public class TokenResponse
(string token, ulong id, string role)
{
    public string Token{get; set;} = token;
    public ulong Id{get; set;} = id;
    public string Role {get; set;} = role;
}