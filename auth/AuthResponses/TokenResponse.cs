

using BackendApp.Model.Enums;

namespace BackendApp.Auth;

public class TokenResponse
(string token, ulong id, UserRole role)
{
    public string Token{get; set;} = token;
    public ulong Id{get; set;} = id;
    public UserRole Role {get; set;} = role;
}