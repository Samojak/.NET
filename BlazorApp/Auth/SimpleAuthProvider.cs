using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging.Console;
using ApiContracts;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
private readonly HttpClient httpClient;
private ClaimsPrincipal currentClaimsPrincipal;

public SimpleAuthProvider(HttpClient httpClient)
{
    this.httpClient = httpClient;
}


public async Task Login(string userName, string password)
{
    HttpResponseMessage response = await httpClient.PostAsJsonAsync(
        "auth/login", new LoginRequest
        {
            UserName = userName,
            Password = password
        });

    string content = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
    {
        throw new Exception(content);
    }

    UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    })!;

    List<Claim> claims = new List<Claim>()
    {
        new Claim(ClaimTypes.Name, userDto.UserName),
        new Claim("UserName", userDto.UserName)
    };

    ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
    currentClaimsPrincipal = new ClaimsPrincipal(identity);

    NotifyAuthenticationStateChanged(
        Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
}




public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return new AuthenticationState(currentClaimsPrincipal ?? new ());
        
    }

    public void Logout()
    {
        currentClaimsPrincipal = new();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }
    
    
    
    
    
}