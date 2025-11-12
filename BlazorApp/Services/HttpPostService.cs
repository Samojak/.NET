using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorApp.Services;

using ApiContracts;


public class HttpPostService : IPostService
{
    private readonly HttpClient client;
    
    public HttpPostService (HttpClient client)
    { 
        this.client = client;
    }


    public async Task<PostDto> AddPostAsync(PostDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("post", request);
        
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
            
    public async Task UpdatePostAsync(int id, PostDto request)
    { 
        try
        {
            var resp = await client.PutAsJsonAsync($"post/{id}", request);
            resp.EnsureSuccessStatusCode(); // 204 on success
        }
        catch (HttpRequestException e)
        {
            // Optionally rethrow with server message if present
            throw new Exception($"Update failed for post {id}: {e.Message}", e);
        }
    }
    
    
    public async Task<PostDto> GetPostAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"post/{id}");
        
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        if (!httpResponse.IsSuccessStatusCode)
        { 
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task DeletePostAsync(int id)
    {
        try
        {
            var resp = await client.DeleteAsync($"post/{id}");
            resp.EnsureSuccessStatusCode(); // 204 on success
        }
        catch (HttpRequestException e)
        {
            // Optionally rethrow with server message if present
            throw new Exception($"Delete failed for post {id}: {e.Message}", e);
        }
    }
    
    
    public async Task<List<PostDto>> GetAllPostsAsync(string? titleContains = null, int? userIdContains = null)
    {
        string url = "post";

        // Build query only if needed, with explicit names
        if (!string.IsNullOrWhiteSpace(titleContains) || userIdContains.HasValue)
        {
            var query = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(titleContains)) query["TitleContains"] = titleContains;
            if (userIdContains.HasValue) query["userIdContains"] = userIdContains.Value.ToString();
            url = QueryHelpers.AddQueryString(url, query);
        }

        var resp = await client.GetAsync(url);
        resp.EnsureSuccessStatusCode();

        return await resp.Content.ReadFromJsonAsync<List<PostDto>>(
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
               ?? new List<PostDto>();
    }


}