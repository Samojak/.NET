using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(PostDto request);
    public Task UpdatePostAsync(int id, PostDto request);

    public Task<PostDto> GetPostAsync(int id);
    public Task DeletePostAsync(int id);
    public Task<List<PostDto>> GetAllPostsAsync(string? titleContains = null, int? userIdContains = null);

}