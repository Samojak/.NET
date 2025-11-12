using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddCommentAsync(CommentDto request)
    {
        var response = await client.PostAsJsonAsync("comment", request);
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<CommentDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateCommentAsync(int id, CommentDto request)
    {
        var response = await client.PutAsJsonAsync($"comment/{id}", request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteCommentAsync(int id)
    {
        var response = await client.DeleteAsync($"comment/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        var response = await client.GetAsync($"comment/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<CommentDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<CommentDto>> GetAllCommentsAsync(int? userId = null, int? postId = null)
    {
        string url = "comment";

        if (userId.HasValue || postId.HasValue)
        {
            var query = new Dictionary<string, string>();
            if (userId.HasValue) query["userId"] = userId.Value.ToString();
            if (postId.HasValue) query["postId"] = postId.Value.ToString();
            url = QueryHelpers.AddQueryString(url, query);
        }

        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<CommentDto>>(
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
               ?? new List<CommentDto>();
    }
}
