using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(PostDto request);
    public Task UpdatePostAsync(int id, PostDto request);
    public Task DeletePostAsync(int id);
    public Task<UserDto> GetPostAsync(int id);
}