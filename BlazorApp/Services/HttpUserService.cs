using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;
    
    public HttpUserService (HttpClient client)
    { 
        this.client = client;
    }


    public async Task<UserDto> AddUserAsync(UserDto request)
    {
        var resp = await client.PostAsJsonAsync("user", request);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<UserDto>(
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
               ?? throw new Exception("Empty response from server.");
    }

    public async Task UpdateUserAsync(int id, UserDto request)
    { 
        try
        {
            var resp = await client.PutAsJsonAsync($"user/{id}", request);
            resp.EnsureSuccessStatusCode(); // 204 on success
        }
        catch (HttpRequestException e)
        {
            // Optionally rethrow with server message if present
            throw new Exception($"Update failed for user {id}: {e.Message}", e);
        }
    }
    
    public async Task<UserDto> GetUserAsync(int id)
    {
        var resp = await client.GetAsync($"user/{id}");
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<UserDto>(
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) 
               ?? throw new Exception("Empty response from server.");
    }

    public async Task DeleteUserAsync(int id)
    {
        try
        {
            var resp = await client.DeleteAsync($"user/{id}");
            resp.EnsureSuccessStatusCode(); // 204 on success
        }
        catch (HttpRequestException e)
        {
            // Optionally rethrow with server message if present
            throw new Exception($"Delete failed for user {id}: {e.Message}", e);
        }
    }
    
    
    public async Task<List<UserDto>> GetAllUsersAsync(string? nameContains = null)
    {
        var url = string.IsNullOrWhiteSpace(nameContains) ? "user"
            : $"user?nameContains={Uri.EscapeDataString(nameContains)}";

        var resp = await client.GetAsync(url);
        resp.EnsureSuccessStatusCode();

        return await resp.Content.ReadFromJsonAsync<List<UserDto>>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
    }

}