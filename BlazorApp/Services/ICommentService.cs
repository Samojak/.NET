using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CommentDto request);
    Task UpdateCommentAsync(int id, CommentDto request);
    Task DeleteCommentAsync(int id);
    Task<CommentDto> GetCommentAsync(int id);
    Task<List<CommentDto>> GetAllCommentsAsync(int? userId = null, int? postId = null);
}