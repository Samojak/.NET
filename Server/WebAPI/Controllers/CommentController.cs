using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController
{
    private readonly ICommentRepository commentRepo;

    public CommentController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    // POST /comment
    [HttpPost]
    public async Task<IResult> AddComment([FromBody] CommentDto request)
    {
        var comment = new Comment
        {
            Body   = request.Body,
            PostId = request.PostId,
            UserId = request.UserId
        };

        var created = await commentRepo.AddAsync(comment);
        return Results.Created($"/comment/{created.Id}", ToDto(created));
    }

    // GET /comment/{commentId}
    [HttpGet("{commentId}")]
    public async Task<IResult> GetSingleComment([FromRoute] int commentId)
    {
        try
        {
            var result = await commentRepo.GetSingleAsync(commentId);
            return Results.Ok(ToDto(result));
        }
        catch (Exception e)
        {
            return Results.NotFound(e.Message);
        }
    }

    // DELETE /comment/{commentId}
    [HttpDelete("{commentId}")]
    public async Task<IResult> DeleteComment([FromRoute] int commentId)
    {
        await commentRepo.DeleteAsync(commentId);
        return Results.NoContent();
    }

    // PUT /comment/{commentId}
    [HttpPut("{commentId}")]
    public async Task<IResult> UpdateComment([FromRoute] int commentId, [FromBody] CommentDto request)
    {
        var comment = new Comment
        {
            Id     = commentId,
            Body   = request.Body,
            PostId = request.PostId,
            UserId = request.UserId
        };

        await commentRepo.UpdateAsync(comment);
        return Results.NoContent();
    }

    // GET /comment?userId=...&postId=...
    [HttpGet]
    public IResult GetAllComments([FromQuery] int? userId, [FromQuery] int? postId)
    {
        var comments = commentRepo.GetMany();

        if (userId.HasValue)
            comments = comments.Where(c => c.UserId == userId.Value);
        if (postId.HasValue)
            comments = comments.Where(c => c.PostId == postId.Value);

        var dtos = comments.Select(ToDto).ToList();
        return Results.Ok(dtos);
    }

    // Helper: map entity → DTO
    private static CommentDto ToDto(Comment c) => new()
    {
        Body   = c.Body,
        PostId = c.PostId,
        UserId = c.UserId
    };
}
